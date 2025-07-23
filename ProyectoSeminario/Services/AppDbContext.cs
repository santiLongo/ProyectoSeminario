using ProyectoSeminario.Models;
using Microsoft.EntityFrameworkCore;


namespace ProyectoSeminario.Services
{

    //Clase del contexto de la Base de Datos
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        // Tablas de la base MySql
        public DbSet<UsuarioDAO> Usuarios { get; set; }
        public DbSet<VehiculoDAO> Vehiculos { get; set; }
        public DbSet<SistemaGpsDAO> Localizadores { get; set; }
        public DbSet<Coordenada> Coordenadas { get; set; }

        //Configuracion de la Base de Datos
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
          //  => options.UseMySql("Server=localhost;Database=truckalizador;User=SantiAdmin;Password=599SzekjcKNva8b",
            //                    new MySqlServerVersion(new Version(9, 3, 0)));
    }
}
