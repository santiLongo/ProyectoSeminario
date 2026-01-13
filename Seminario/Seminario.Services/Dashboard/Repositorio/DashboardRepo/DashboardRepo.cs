using System.Data;
using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.Dashboard.GetHome.Model;

namespace Seminario.Services.Dashboard.Repositorio.DashboardRepo
{
    public interface IDashboardRepo
    {
        Task<Cabezera> GetCabezera();
        Task<Alertas> GetAlertas();
        Task<Cards> GetCards();
        Task<List<Viaje>> GetViajes();
        Task<List<Mantenimiento>> GetMantenimientos();
        Task<Finanzas> GetFinanzas();
    }
    public class DashboardRepo(IDbConnection connection) : IDashboardRepo
    {
        private readonly IDbConnection _connection = connection;
        public async Task<Cabezera> GetCabezera()
        {
            var sql = @"CALL GetCabezeraDashboard();";
            //
            return await _connection.ExecuteScalarAsync<Cabezera>(sql);
        }

        public Task<Alertas> GetAlertas()
        {
            throw new NotImplementedException();
        }

        public Task<Cards> GetCards()
        {
            throw new NotImplementedException();
        }

        public Task<List<Viaje>> GetViajes()
        {
            throw new NotImplementedException();
        }

        public Task<List<Mantenimiento>> GetMantenimientos()
        {
            throw new NotImplementedException();
        }

        public Task<Finanzas> GetFinanzas()
        {
            throw new NotImplementedException();
        }
    }
}
