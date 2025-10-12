using Dapper;
using Microsoft.EntityFrameworkCore;
using ProyectoSeminario.Commands.MaestroCliente.Models;
using ProyectoSeminario.Entitys;
using ProyectoSeminario.Models.Usuario;
using ProyectoSeminario.Models.Vehiculo;
using ProyectoSeminario.Repository.Cliente;
using System.Data;


namespace ProyectoSeminario.Services
{
    //Clase del contexto de la Base de Datos
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // Tablas de la base MySql
        public DbSet<UsuarioDAO> Usuarios { get; set; }
        public DbSet<VehiculoDAO> Vehiculos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public IRepositoryCliente ClienteRepo => new RepositoryCliente(this);

        public async Task<IEnumerable<T>> ExecuteAsync<T>(string query, DynamicParameters parameters)
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            return await connection.QueryAsync<T>(query, parameters);

        }

    }
}
