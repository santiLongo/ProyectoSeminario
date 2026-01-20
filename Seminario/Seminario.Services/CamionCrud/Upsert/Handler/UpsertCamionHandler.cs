using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.CamionCrud.Upsert.Command;

namespace Seminario.Services.CamionCrud.Upsert.Handler;

public class UpsertCamionHandler
{
    private readonly IAppDbContext _ctx;
    
    public UpsertCamionHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(UpsertCamionCommand command)
    {
        var tipo = await _ctx.TipoCamionRepo.GetByIdAsync(command.IdTipoCamion);

        if (tipo == null)
        {
            throw new InvalidOperationException($"No se encuentra el Tipo de Camion {command.IdTipoCamion}");
        }
        //
        var camion = await _ctx.CamionRepo.GetAsync(q => q.WhereEqualsIdCamion(command.idCamion));

        if (camion == null)
        {
            camion = Camion.Create();
            _ctx.CamionRepo.Add(camion);
            camion.FechaAlta =  DateTime.Today;
        }
        
        camion.IdTipoCamion = command.IdTipoCamion;
        camion.Marca = command.Marca.ToUpper();
        camion.Modelo = command.Modelo.ToUpper();
        camion.Patente = command.Patente.ToUpper();
        camion.NroChasis = command.NroChasis.ToUpper();
        camion.NroMotor = command.NroMotor.ToUpper();

        await _ctx.SaveChangesAsync();
    }
}