using System.Net;
using Microsoft.EntityFrameworkCore;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.CobrosServices.Update.Command;

namespace Seminario.Services.CobrosServices.Update.Handler;

public class CobrosUpdateHandler
{
    private readonly IAppDbContext _ctx;
    private const int Pesos = 1;
    private bool _seguridad;
    
    public CobrosUpdateHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(CobrosUpdateCommand command)
    {
        _seguridad = command.Seguridad;
        
        if(command.IdMoneda != Pesos)
            if (command.TipoCambio != null)
                throw new SeminarioException("EL tipo de cambio es obligatorio cuando la moneda no es pesos",
                    HttpStatusCode.BadRequest);      
                
        var cobro = await _ctx.CobrosRepo.FindByIdCobroAsync(command.IdCobro);
        
        if (command.IdFormaPago != 1)
        {
            if (command.DatosCheque!.IdCheque != null)
            {
                await ValidoChequeUpdate(command.DatosCheque);
            }
            else
            {
                cobro.Cheque = await ValidoChequeAdd(command.DatosCheque);
            }
        }
        
        cobro.Monto = command.Monto.GetValueOrDefault();
        cobro.IdMoneda = command.IdMoneda.GetValueOrDefault();
        cobro.TipoCambio = command.TipoCambio.GetValueOrDefault();
        cobro.IdFormaPago = command.IdFormaPago.GetValueOrDefault();
            
        await _ctx.SaveChangesAsync();
        await _ctx.ViajeRepo.ActualizarEstadoAsync(cobro.IdViaje);
    }

    private async Task ValidoChequeUpdate(CobroChequeCommand datos)
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
            .FindByIdAsync(datos.IdCheque.GetValueOrDefault(), includePago: true);
        
        if (cheque == null)
            throw new SeminarioException($"El cheque informado no se encontro", HttpStatusCode.NotFound);

        if (cheque.Pago != null && _seguridad)
            throw new SeminarioException(
                $"El cheque informado {cheque.NroCheque} se utilizo para realizar un pago, necesita seguridad para modificarlo");

        cheque.NroCheque = datos.NroCheque.GetValueOrDefault();
        cheque.CuitEmisor = datos.CuitEmisor.GetValueOrDefault();
        cheque.IdBanco = banco.IdBanco;
        cheque.FechaCobro = datos.FechaCobro;
        cheque.FechaVencimiento = datos.FechaCobro.GetValueOrDefault().AddDays(30);
    }
    
    private async Task<PagoCheque?> ValidoChequeAdd(CobroChequeCommand? datos)
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