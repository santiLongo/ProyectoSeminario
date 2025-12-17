using Seminario.Datos.Entidades.Cliente;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Entidades.Usuario;
using Seminario.Datos.Repositorios.ClienteRepo;
using Seminario.Datos.Repositorios.UsuarioRepo;

namespace Seminario.Datos.Contextos.AppDbContext
{
    public interface IAppDbContext
    {
        public IRepoCliente ClienteRepo { get;}
        public IUsuarioRepo UsuarioRepo { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #region Entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        #endregion

        #region Repos
        public IRepoCliente ClienteRepo => new RepoCliente(this);
        public IUsuarioRepo UsuarioRepo => new UsuarioRepo(this);

        #endregion


        public async Task<IEnumerable<T>> ExecuteAsync<T>(string query, DynamicParameters parameters)
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            return await connection.QueryAsync<T>(query, parameters);
        }
    }
}
