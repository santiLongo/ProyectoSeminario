using System.Net;
using Microsoft.EntityFrameworkCore;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.CobrosServices.Add.Command;

namespace Seminario.Services.CobrosServices.Add.Handler;

public class AddCobroHandler
{
    private readonly IAppDbContext _ctx;
    private const int Pesos = 1;
    
    public AddCobroHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(AddCobroCommand command)
    {
        if(command.IdMoneda != Pesos)
            if (command.TipoCambio != null)
                throw new SeminarioException("EL tipo de cambio es obligatorio cuando la moneda no es pesos",
                    HttpStatusCode.BadRequest);      
                        
        var viaje = await ValidoViaje(command.IdViaje.GetValueOrDefault());
        
        var cobro = new Cobro
        {
            IdViaje = command.IdViaje.GetValueOrDefault(),
            FechaRecibo = command.FechaRecibido.GetValueOrDefault(),
            Monto = command.Monto.GetValueOrDefault(),
            IdMoneda = command.IdMoneda.GetValueOrDefault(),
            TipoCambio = command.TipoCambio.GetValueOrDefault(),
            IdFormaPago = command.IdFormaPago.GetValueOrDefault(),
            Viaje = viaje,
        };
        
        if(command.IdFormaPago == 1)
        {
            cobro.Cheque = await ValidoCheque(command.DatosCheque);
        }
        
        viaje.Cobros.Add(cobro);

        _ctx.ViajeRepo.ForzarModifiedTrigger(viaje);
            
        await _ctx.SaveChangesAsync();
    }

    private async Task<Viaje> ValidoViaje(int idViaje)
    {
        var viaje = await _ctx.ViajeRepo.Query()
            .IncludeCobros()
            .FirstOrDefaultAsync(v => v.IdViaje == idViaje);

        if (viaje == null) 
            throw new InvalidOperationException("El viaje no existe");
        
        if(viaje.Estado == EstadosViaje.Cobrado.ToInt()) 
            throw new InvalidOperationException("El viaje ya se encuentra cobrado");
        
        return viaje;
    }

    private async Task<PagoCheque?> ValidoCheque(CobroChequeCommand? datos)
    {
        if (datos == null)
            throw new InvalidOperationException("Faltan los datos del cheque");
        
        if (datos.NroCheque == null)
            throw new InvalidOperationException("El numero de cheque es obligatorio");

        if (datos.CuitEmisor == null)
            throw new InvalidOperationException("El cuit del emisor es requerido");

        if (datos.FechaCobro == null)
            throw new InvalidOperationException("La fecha de cobro del cheque es obligatoria");

        if (datos.IdBanco == null)
            throw new InvalidOperationException("El banco es obligatorio para cargar el cheque");
        
        var banco = await _ctx.BancoRepo.GetByIdAsync(datos!.IdBanco.GetValueOrDefault());
        
        if (banco == null)
            throw new InvalidOperationException("El banco no esta ingresado en el sistema");
        
        var cheque = await _ctx.PagoChequeRepo
            .FindByNroYBancoAsync(datos.NroCheque.GetValueOrDefault(), datos.IdBanco.GetValueOrDefault());
        
        if (cheque != null)
            throw new InvalidOperationException($"El cheque {cheque.NroCheque} de {cheque.CuitEmisor} informado ya se encuentra registrado");
        
       

        cheque = new PagoCheque
        {
            NroCheque = datos.NroCheque.GetValueOrDefault(),
            EsPropio = false,
            CuitEmisor = datos.CuitEmisor.GetValueOrDefault(),
            IdBanco = banco.IdBanco,
            FechaCobro = datos.FechaCobro,
            FechaVencimiento = datos.FechaCobro.GetValueOrDefault().AddDays(30),
            Rechazado = false,
        };
        
        return cheque;
    }
}