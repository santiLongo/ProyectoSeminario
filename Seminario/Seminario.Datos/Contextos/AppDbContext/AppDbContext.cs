using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;

namespace Seminario.Datos.Contextos.AppDbContext
{
    public interface IAppDbContext
    {
        IClienteRepo ClienteRepo { get;}
        IUsuarioRepo UsuarioRepo { get; }
        IBancoRepo BancoRepo { get; }
        ITipoCamionRepo TipoCamionRepo { get; }
        IUbicacionRepo UbicacionRepo { get; }
        ICamionRepo  CamionRepo { get; }
        IChoferRepo ChoferRepo { get; }
        IViajeRepo ViajeRepo { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    public partial class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        #region Entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Banco> Bancos { get; set; }

        public DbSet<Camion> Camiones { get; set; }

        public DbSet<Chofer> Choferes { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Cobro> Cobros { get; set; }

        public DbSet<CobroCheque> CobroCheques { get; set; }

        public DbSet<CompraRepuesto> CompraRepuestos { get; set; }

        public DbSet<CompraRepuestoDetalle> CompraRepuestoDetalles { get; set; }

        public DbSet<Especialidad> Especialidades { get; set; }

        public DbSet<FormaPago> FormapPagos { get; set; }

        public DbSet<Localidad> Localidades { get; set; }

        public DbSet<Mantenimiento> Mantenimientos { get; set; }

        public DbSet<MantenimientoObservacion> MantenimientoObservaciones { get; set; }
            
        public DbSet<MantenimientoTarea> MantenimientoTareas { get; set; }

        public DbSet<Moneda> Monedas { get; set; }

        public DbSet<Pago> Pagos { get; set; }

        public DbSet<PagoCompraRepuesto> PagoCompraRepuestos { get; set; }

        public DbSet<PagoMantenimiento> PagoMantenimientos { get; set; }

        public DbSet<PagoCheque> PagoCheques { get; set; }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<ProveedorEspecialidad> ProveedorEspecialidades { get; set; }

        public DbSet<Provincia> Provincias { get; set; }

        public DbSet<Taller> Talleres { get; set; }

        public DbSet<TallerEspecialidad> TallerEspecialidades { get; set; }

        public DbSet<TipoCamion> TipoCamiones { get; set; }

        public DbSet<Viaje> Viajes { get; set; }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Banco>(entity =>
            {
                entity.HasKey(e => e.IdBanco).HasName("PRIMARY");
            });

            modelBuilder.Entity<Camion>(entity =>
            {
                entity.HasKey(e => e.IdCamion).HasName("PRIMARY");

                entity.Property(e => e.Marca).IsFixedLength();
                entity.Property(e => e.Modelo).IsFixedLength();
                entity.Property(e => e.Patente).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.TipoCamion).WithMany(p => p.Camiones).HasConstraintName("FK_CAMION_TIPOCAMION");
            });

            modelBuilder.Entity<Chofer>(entity =>
            {
                entity.HasKey(e => e.IdChofer).HasName("PRIMARY");

                entity.Property(e => e.Apellido).IsFixedLength();
                entity.Property(e => e.Direccion).IsFixedLength();
                entity.Property(e => e.Nombre).IsFixedLength();
                entity.Property(e => e.NroRegistro).HasComment("Nro Registro Profecional");
                entity.Property(e => e.UserName).IsFixedLength();
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente).HasName("PRIMARY");

                entity.Property(e => e.UserName).IsFixedLength();
            });

            modelBuilder.Entity<Cobro>(entity =>
            {
                entity.HasKey(e => e.IdCobro).HasName("PRIMARY");

                entity.HasOne(d => d.FormaPago).WithMany(p => p.Cobros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBRO_FORMAPAGO");

                entity.HasOne(d => d.Moneda).WithMany(p => p.Cobros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBRO_MONEDA");

                entity.HasOne(d => d.Viaje).WithMany(p => p.Cobros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBRO_VIAJE");
            });

            modelBuilder.Entity<CobroCheque>(entity =>
            {
                entity.HasKey(e => e.IdCobroCheque).HasName("PRIMARY");

                entity.HasOne(d => d.Banco).WithMany(p => p.CobroCheques)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBROCHEQUE_BANCO");

                entity.HasOne(d => d.Cobro).WithMany(p => p.CobroCheques).HasConstraintName("FK_COBROCHEQUE_COBRO");
            });

            modelBuilder.Entity<CompraRepuesto>(entity =>
            {
                entity.HasKey(e => e.IdCompraRepuesto).HasName("PRIMARY");

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.CompraRepuestos).HasConstraintName("FK_COMPRAREPUESTO_MANTENIMIENTO");

                entity.HasOne(d => d.Proveedor).WithMany(p => p.CompraRepuestos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMPRAREPUESTO_PROVEEDOR");
            });

            modelBuilder.Entity<CompraRepuestoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");

                entity.HasOne(d => d.CompraRepuesto).WithMany(p => p.Detalles).HasConstraintName("FK_DETALLE_COMPRA/REPUESTO");
            });

            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.IdEspecialidad).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
            });

            modelBuilder.Entity<FormaPago>(entity =>
            {
                entity.HasKey(e => e.IdFormaPago).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
            });

            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.HasKey(e => e.IdLocalidad).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Provincia).WithMany(p => p.Localidades).HasConstraintName("FK_LOCALIDAD_PROVINCIA");
            });

            modelBuilder.Entity<Mantenimiento>(entity =>
            {
                entity.HasKey(e => e.IdMantenimiento).HasName("PRIMARY");

                entity.Property(e => e.Titulo).IsFixedLength();

                entity.HasOne(d => d.Taller).WithMany(p => p.Mantenimientos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MANTENIMIENTO_TALLER");

                entity.HasOne(d => d.Vehiculo).WithMany(p => p.Mantenimientos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MANTENIMIENTO_CAMION");
            });

            modelBuilder.Entity<MantenimientoObservacion>(entity =>
            {
                entity.HasKey(e => e.IdObservacion).HasName("PRIMARY");

                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.Observaciones).HasConstraintName("FK_MANTENIMIENTO/OBSERVACION_MANTENIEMIENTO");
            });

            modelBuilder.Entity<MantenimientoTarea>(entity =>
            {
                entity.HasKey(e => e.IdTarea).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.Tareas).HasConstraintName("FK_MANTENIMIENTO/TAREA_MANTENIEMIENTO");
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasKey(e => e.IdMoneda).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.IdPago).HasName("PRIMARY");

                entity.Property(e => e.Concepto).IsFixedLength();

                entity.HasOne(d => d.FormaPago).WithMany(p => p.Pagos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGO_FORMAPAGO");

                entity.HasOne(d => d.Moneda).WithMany(p => p.Pagos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGO_MONEDA");
            });

            modelBuilder.Entity<PagoCompraRepuesto>(entity =>
            {
                entity.HasKey(e => e.IdPagoCompraRepuesto).HasName("PRIMARY");

                entity.HasOne(d => d.CompraRepuesto).WithMany(p => p.PagoCompraRepuestos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCOMPRA_COMPRAREPUESTO");

                entity.HasOne(d => d.Pago).WithMany(p => p.PagoCompraRepuestos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCOMPRA_PAGO");
            });

            modelBuilder.Entity<PagoMantenimiento>(entity =>
            {
                entity.HasKey(e => e.IdPagoMantenimiento).HasName("PRIMARY");

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.PagoMantenimientos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOMANTENIMIENTO_MANTENIMIENTO");

                entity.HasOne(d => d.Pago).WithMany(p => p.PagoMantenimientos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOMANTENIMIENTO_PAGO");
            });

            modelBuilder.Entity<PagoCheque>(entity =>
            {
                entity.HasKey(e => e.IdPagoCheque).HasName("PRIMARY");

                entity.HasOne(d => d.Banco).WithMany(p => p.PagoCheques)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCHEQUE_BANCO");

                entity.HasOne(d => d.Pago).WithMany(p => p.PagoCheques)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCHEQUE_PAGO");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor).HasName("PRIMARY");

                entity.Property(e => e.Direccion).IsFixedLength();
                entity.Property(e => e.Mail).IsFixedLength();
                entity.Property(e => e.RazonSocial).IsFixedLength();
                entity.Property(e => e.Responsable).IsFixedLength();

                entity.HasOne(d => d.Localidad).WithMany(p => p.Proveedores)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVEEDOR_LOCALIDAD");
            });

            modelBuilder.Entity<ProveedorEspecialidad>(entity =>
            {
                entity.HasKey(e => e.IdProveedorEspecialidad).HasName("PRIMARY");

                entity.HasOne(d => d.Especialidad).WithMany(p => p.ProveedorEspecialidades)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVEEDOR/ESPECIALIDAD_ESPECIALIDAD");

                entity.HasOne(d => d.Proveedor).WithMany(p => p.ProveedorEspecialidades)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVEEDOR/ESPECIALIDAD_PROVEEDOR");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.IdProvincia).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Pais).WithMany(p => p.Provincias).HasConstraintName("FK_PROVINCIA_PAIS");
            });

            modelBuilder.Entity<Taller>(entity =>
            {
                entity.HasKey(e => e.IdTaller).HasName("PRIMARY");

                entity.Property(e => e.Mail).IsFixedLength();
                entity.Property(e => e.Nombre).IsFixedLength();
                entity.Property(e => e.Responsable).IsFixedLength();

                entity.HasOne(d => d.Localidad).WithMany(p => p.Talleres)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALLER_LOCALIDAD");
            });

            modelBuilder.Entity<TallerEspecialidad>(entity =>
            {
                entity.HasKey(e => e.IdTallerEspecialidad).HasName("PRIMARY");

                entity.Property(e => e.IdTallerEspecialidad).ValueGeneratedNever();

                entity.HasOne(d => d.Especialidad).WithMany(p => p.TallerEspecialidades)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALLER/ESPECIALIDAD_ESPECIALIDAD");

                entity.HasOne(d => d.Taller).WithMany(p => p.TallerEspecialidades)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALLER/ESPECIALIDAD_TALLER");
            });

            modelBuilder.Entity<TipoCamion>(entity =>
            {
                entity.HasKey(e => e.IdTipoCamion).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();
            });

            modelBuilder.Entity<Viaje>(entity =>
            {
                entity.HasKey(e => e.IdViaje).HasName("PRIMARY");

                entity.Property(e => e.Carga).IsFixedLength();
                entity.Property(e => e.UserAlta).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Camion).WithMany(p => p.Viajes)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VIAJE_CAMION");

                entity.HasOne(d => d.Chofer).WithMany(p => p.Viajes)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VIAJE_CHOFER");

                entity.HasOne(d => d.Cliente).WithMany(p => p.Viajes)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VIAJE_CLIENTE");
            });

            modelBuilder.Entity<Destino>(entity =>
            {
                entity.HasKey(e => e.IdDestino);

                entity.HasOne(d => d.Viaje).WithMany(p => p.Destinos)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Localidad).WithMany(p => p.Destinos)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            
            modelBuilder.Entity<Procedencia>(entity =>
            {
                entity.HasKey(e => e.IdProcedencia);

                entity.HasOne(d => d.Viaje).WithMany(p => p.Procendecias)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Localidad).WithMany(p => p.Procedencias)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #endregion

        #region Repos
        public IClienteRepo ClienteRepo => new ClienteRepo(this);
        public IUsuarioRepo UsuarioRepo => new UsuarioRepo(this);
        public IBancoRepo BancoRepo => new BancoRepo(this);
        public ITipoCamionRepo TipoCamionRepo => new TipoCamionRepo(this);
        public IUbicacionRepo UbicacionRepo => new UbicacionRepo(this);
        public ICamionRepo CamionRepo => new CamionRepo(this);
        public IChoferRepo ChoferRepo => new ChoferRepo(this);
        public IViajeRepo ViajeRepo => new ViajeRepo(this);
        #endregion
    }
}
