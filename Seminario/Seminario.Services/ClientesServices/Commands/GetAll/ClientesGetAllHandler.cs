using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.Entidades;

namespace Seminario.Services.ClientesServices.Commands.GetAll;

public class ClientesGetAllHandler
{
    private readonly IDbExecutor _executor;

    public ClientesGetAllHandler(IDbExecutor executor)
    {
        _executor = executor;
    }

    public async Task<IEnumerable<ClientesGetAllResponse>> HandleAsync(ClientesGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@estado", (int)command.Estado);
        //
        var sql = @"
                    Select
                        idCliente AS IdCliente,
                        cuit AS Cuit,
                        razonSocial AS RazonSocial,
                        direccion AS Direccion,
                        telefono AS Telefono,
                        mail AS Email,
                        FechaBaja As Fechabaja,
                        userName AS UserName,
                        userDateTime AS UserDateTime
                    FRom cliente
                    where 
                    (
                        (@estado = 1 AND FechaBaja is null) OR
                        (@estado = 2 AND FechaBaja is not null) OR   
                        (@estado = 3)
                    )";
        //
        return await _executor.ExecuteAsync<ClientesGetAllResponse>(sql, p);
    }
}

public class ClientesGetAllCommand
{
    public ClienteGetAllEstados Estado { get; set; }
}

public enum ClienteGetAllEstados
{
    Activos = 1,
    Inactivos = 2,
    Todos = 3
}