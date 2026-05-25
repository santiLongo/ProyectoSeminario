using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.RecibosServices.AddRecibo;

public class AddReciboHandler
{
    private readonly IAppDbContext _ctx;

    public AddReciboHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(AddReciboCommand command)
    {
        if (command.TipoRecibo != 1 && command.TipoRecibo != 2)
            throw new InvalidOperationException("TipoRecibo debe ser 1 (Cobro) o 2 (Pago)");

        // Validar contraparte
        await ValidarContraparteAsync(command);

        // Calcular monto total del recibo
        var montoTotal = command.FormasDePago.Sum(fp => fp.Monto!.Value);

        if (montoTotal <= 0)
            throw new InvalidOperationException("El monto total del recibo debe ser mayor a cero");

        // Construir recibo
        var recibo = new Recibo
        {
            Tipo = (Recibo.TipoRecibo)command.TipoRecibo!.Value,
            FechaRecibo = command.FechaRecibo!.Value,
            MontoTotal = montoTotal,
            IdMoneda = command.IdMoneda!.Value,
            TipoCambio = command.TipoCambio.HasValue ? (double?)command.TipoCambio.Value : null,
            IdCliente = command.IdCliente,
            IdProveedor = command.IdProveedor,
            IdTaller = command.IdTaller,
            Observaciones = command.Observaciones,
            Anulado = false
        };

        // Agregar formas de pago
        foreach (var fpCmd in command.FormasDePago)
        {
            var formaPago = new ReciboFormaPago
            {
                IdFormaPago = fpCmd.IdFormaPago!.Value,
                Monto = fpCmd.Monto!.Value
            };

            if (fpCmd.DatosCheque != null)
            {
                formaPago.PagoCheque = await CrearChequeAsync(fpCmd.DatosCheque);
            }

            recibo.FormasDePago.Add(formaPago);
        }

        // Validar y agregar imputaciones
        decimal totalImputado = 0;
        var facturasARecalcular = new List<int>();

        foreach (var impCmd in command.Imputaciones)
        {
            var factura = await _ctx.FacturaRepo.Query()
                .IncludeRecibos()
                .FirstOrDefaultAsync(f => f.IdFactura == impCmd.IdFactura!.Value);

            if (factura == null)
                throw new InvalidOperationException($"La factura {impCmd.IdFactura} no existe");

            if (factura.Anulada)
                throw new InvalidOperationException($"La factura {impCmd.IdFactura} está anulada y no puede imputarse");

            var saldoPendiente = factura.Total - factura.ReciboFacturas
                .Where(rf => rf.Recibo != null && !rf.Recibo.Anulado)
                .Sum(rf => rf.ImporteAplicado);

            if (impCmd.ImporteAplicado!.Value > saldoPendiente)
                throw new InvalidOperationException(
                    $"El importe aplicado ({impCmd.ImporteAplicado}) supera el saldo pendiente ({saldoPendiente}) de la factura {impCmd.IdFactura}");

            recibo.ReciboFacturas.Add(new ReciboFactura
            {
                IdFactura = impCmd.IdFactura!.Value,
                ImporteAplicado = impCmd.ImporteAplicado!.Value
            });

            totalImputado += impCmd.ImporteAplicado!.Value;
            facturasARecalcular.Add(factura.IdFactura);
        }

        _ctx.ReciboRepo.Add(recibo);
        await _ctx.SaveChangesAsync();

        // Recalcular estado de las facturas imputadas
        foreach (var idFactura in facturasARecalcular)
        {
            await _ctx.FacturaRepo.RecalcularEstadoAsync(idFactura);
        }

        if (facturasARecalcular.Any())
            await _ctx.SaveChangesAsync();

        return recibo.IdRecibo;
    }

    private async Task ValidarContraparteAsync(AddReciboCommand command)
    {
        var tieneContraparte = command.IdCliente.HasValue
            || command.IdProveedor.HasValue
            || command.IdTaller.HasValue;

        if (!tieneContraparte)
            throw new InvalidOperationException("Se debe informar al menos una contraparte (cliente, proveedor o taller)");

        if (command.IdCliente.HasValue)
        {
            var cliente = await _ctx.ClienteRepo.FindByIdAsync(command.IdCliente.Value);
            if (cliente == null)
                throw new InvalidOperationException("El cliente informado no existe");
        }

        if (command.IdProveedor.HasValue)
        {
            var proveedor = await _ctx.ProveedorRepo.FindByIdAsync(command.IdProveedor.Value);
            if (proveedor == null)
                throw new InvalidOperationException("El proveedor informado no existe");
        }

        if (command.IdTaller.HasValue)
        {
            var taller = await _ctx.TallerRepo.FindByIdAsync(command.IdTaller.Value);
            if (taller == null)
                throw new InvalidOperationException("El taller informado no existe");
        }
    }

    private async Task<PagoCheque> CrearChequeAsync(AddReciboChequeCommand datos)
    {
        if (datos.NroCheque == null)
            throw new InvalidOperationException("El número de cheque es obligatorio");

        if (datos.CuitEmisor == null)
            throw new InvalidOperationException("El CUIT del emisor es requerido");

        if (datos.FechaCobro == null)
            throw new InvalidOperationException("La fecha de cobro del cheque es obligatoria");

        if (datos.IdBanco == null)
            throw new InvalidOperationException("El banco es obligatorio para cargar el cheque");

        var banco = await _ctx.BancoRepo.GetByIdAsync(datos.IdBanco.Value);
        if (banco == null)
            throw new InvalidOperationException("El banco no está ingresado en el sistema");

        var chequeExistente = await _ctx.PagoChequeRepo
            .FindByNroYBancoAsync(datos.NroCheque.Value, datos.IdBanco.Value);

        if (chequeExistente != null)
            throw new InvalidOperationException($"El cheque {chequeExistente.NroCheque} ya se encuentra registrado");

        return new PagoCheque
        {
            NroCheque = datos.NroCheque.Value,
            EsPropio = datos.EsPropio,
            CuitEmisor = datos.CuitEmisor.Value,
            IdBanco = datos.IdBanco.Value,
            FechaCobro = datos.FechaCobro,
            FechaVencimiento = datos.FechaCobro.Value.AddDays(30),
            Rechazado = false
        };
    }
}
