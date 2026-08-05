using Dapper;
using Microsoft.EntityFrameworkCore;
using Seminario.Core.Dapper;
using Seminario.Datos;
using Seminario.Datos.Enums;
using Seminario.Datos.StoredProcedures;
using Seminario.Services.ViajeServices.GetAll.Command;
using Seminario.Services.ViajeServices.GetAll.Model;

namespace Seminario.Services.ViajeServices.GetAll.Handler;

public class GetAllViajeHandler
{
    private readonly DbExecutor _executor;

    public GetAllViajeHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<List<GetAllViajeModel>> Handle(GetAllViajeCommand command)
    {
        var p = new DynamicParameters();
        //
        p.Add("@nroViaje", command.NroViaje);
        p.Add("@idCamion", command.IdCamion);
        p.Add("@idCliente", command.IdCliente);
        p.Add("@idChofer", command.IdChofer);
        p.Add("@idLocalidadDest", command.IdLocalidadDestino);
        p.Add("@idLocalidadProc", command.IdLocalidadProcedencia);
        p.Add("@fechaAltaDesde", command.FechaAltaDesde);
        p.Add("@fechaAltaHasta", command.FechaAltaHasta);
        p.Add("@estado", command.Estado);
        //
        var sql = Querys.GetAllViajes;

        var result = (await _executor.ExecuteAsync<GetAllViajeModel>(sql, p)).ToList();

        for(int i = 0; i < result.Count; i++)
        {
            result[i].Estado = EstadosViajeDiccionary
                .Estados
                .FirstOrDefault(d => d.Key == Convert.ToInt32(result[i].Estado))
                .Value;
        }

        return result;
    }
}