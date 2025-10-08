using Microsoft.AspNetCore.Identity;
using ProyectoSeminario.Commands.MaestroCliente.Commands.GetAllCommand;
using ProyectoSeminario.Commands.MaestroCliente.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using ProyectoSeminario.Services;

namespace ProyectoSeminario.Commands.MaestroCliente.Handlers.GetAll
{

    public interface IGetAllClienteHandler
    {
        Task<List<ClienteGridModel>> Handle(GetAllClienteCommand command);
    }
    public class GetAllClienteHandler : IGetAllClienteHandler
    {
        private readonly AppDbContext _ctx;

        public GetAllClienteHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<ClienteGridModel>> Handle(GetAllClienteCommand command)
        {
            var p = new DynamicParameters();

            p.Add("@razonSocial", command.RazonSocial);
            p.Add("@cuit", command.Cuit);

            string sql = "SELECT" +
                        "   idCliente AS NumeroCliente," +
                        "   razonSocial AS RazonSocial," +
                        "   cuit AS Cuit," +
                        "   telefono AS Telefono," +
                        "   mail AS Mail," +
                        "   direccion AS Direccion," +
                        "   userAlta AS userAlta," +
                        "   userDateAlta AS FechaAlta" +
                        "FROM cliente " +
                        "WHERE" +
                        "        (@razonSocial IS NULL OR razonSocial LIKE @razonSocial)" +
                        "    AND (@cuit IS NULL OR cuit LIKE @cuit);";

            var result = await _ctx.ExecuteAsync<ClienteGridModel>(sql, p);
            return result.ToList();

        }
    }
}
