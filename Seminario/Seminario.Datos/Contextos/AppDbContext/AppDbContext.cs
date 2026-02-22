using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Entidades;
using Seminario.Datos.Entidades.Interfaces;
using Seminario.Datos.Repositorios;
using Seminario.Datos.Services.CurrentUserService;

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
        IDestinoRepo DestinoRepo { get; }
        IProcedenciaRepo ProcedenciaRepo { get; }
        IMantenimientoRepo MantenimientoRepo { get; }
        IPagoChequeRepo PagoChequeRepo { get; }
        ICobrosRepo CobrosRepo { get; }
        IPagosRepo PagosRepo { get; }
        IEspecialidadRepo EspecialidadRepo { get; }
        ITallerRepo TallerRepo { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
    public partial class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ICurrentUserService _currentUser;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService userService) : base(options)
        {
            _currentUser = userService;
        }

        #region Entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Banco> Bancos { get; set; }

        public DbSet<Camion> Camiones { get; set; }

        public DbSet<Chofer> Choferes { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Cobro> Cobros { get; set; }

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
        
        public DbSet<Destino> Destinos { get; set; }
        
        public DbSet<Procedencia> Procedencias { get; set; }
        
        public DbSet<ViajeObservacion> ViajesObservaciones { get; set; }
            
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

                entity.HasOne(d => d.TipoCamion)
                    .WithMany(p => p.Camiones)
                    .HasForeignKey(f => f.IdTipoCamion)
                    .HasConstraintName("FK_CAMION_TIPOCAMION");
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

                entity.HasOne(d => d.FormaPago)
                    .WithMany(p => p.Cobros)
                    .HasForeignKey(f => f.IdFormaPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBRO_FORMAPAGO");

                entity.HasOne(d => d.Moneda)
                    .WithMany(p => p.Cobros)
                    .HasForeignKey(f => f.IdMoneda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBRO_MONEDA");

                entity.HasOne(d => d.Viaje).WithMany(p => p.Cobros)
                    .HasForeignKey(f => f.IdViaje)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COBRO_VIAJE");

                entity.HasOne(d => d.Cheque).WithOne(p => p.Cobro)
                    .HasForeignKey<Cobro>(f => f.IdPagoCheque)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            
            modelBuilder.Entity<CompraRepuesto>(entity =>
            {
                entity.HasKey(e => e.IdCompraRepuesto).HasName("PRIMARY");

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.CompraRepuestos)
                    .HasForeignKey(f => f.IdMantenimiento)
                    .HasConstraintName("FK_COMPRAREPUESTO_MANTENIMIENTO");

                entity.HasOne(d => d.Proveedor).WithMany(p => p.CompraRepuestos)
                    .HasForeignKey(f => f.IdProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMPRAREPUESTO_PROVEEDOR");
            });

            modelBuilder.Entity<CompraRepuestoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");

                entity.HasOne(d => d.CompraRepuesto).WithMany(p => p.Detalles)
                    .HasForeignKey(f => f.IdCompraRepuesto)
                    .HasConstraintName("FK_DETALLE_COMPRA/REPUESTO");
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

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.Localidades)
                    .HasForeignKey(f => f.IdProvincia)
                    .HasConstraintName("FK_LOCALIDAD_PROVINCIA");
            });

            modelBuilder.Entity<Mantenimiento>(entity =>
            {
                entity.HasKey(e => e.IdMantenimiento).HasName("PRIMARY");

                entity.Property(e => e.Titulo).IsFixedLength();

                entity.HasOne(d => d.Taller).WithMany(p => p.Mantenimientos)
                    .HasForeignKey(f => f.IdTaller)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MANTENIMIENTO_TALLER");

                entity.HasOne(d => d.Vehiculo).WithMany(p => p.Mantenimientos)
                    .HasForeignKey(f => f.IdVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MANTENIMIENTO_CAMION");
            });

            modelBuilder.Entity<MantenimientoObservacion>(entity =>
            {
                entity.HasKey(e => e.IdObservacion).HasName("PRIMARY");

                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.Observaciones)
                    .HasForeignKey(f => f.IdMantenimiento)
                    .HasConstraintName("FK_MANTENIMIENTO/OBSERVACION_MANTENIEMIENTO");
            });

            modelBuilder.Entity<MantenimientoTarea>(entity =>
            {
                entity.HasKey(e => e.IdTarea).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.Tareas)
                    .HasForeignKey(f => f.IdMantenimiento)
                    .HasConstraintName("FK_MANTENIMIENTO/TAREA_MANTENIEMIENTO");
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
                    .HasForeignKey(f => f.IdFormaPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGO_FORMAPAGO");

                entity.HasOne(d => d.Moneda).WithMany(p => p.Pagos)
                    .HasForeignKey(f => f.IdMoneda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGO_MONEDA");
                
                entity.HasOne(d => d.Cheque).WithOne(p => p.Pago)
                    .HasForeignKey<Pago>(f => f.IdPagoCheque)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PagoCompraRepuesto>(entity =>
            {
                entity.HasKey(e => e.IdPagoCompraRepuesto).HasName("PRIMARY");

                entity.HasOne(d => d.CompraRepuesto).WithMany(p => p.PagoCompraRepuestos)
                    .HasForeignKey(f => f.IdCompraRepuesto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCOMPRA_COMPRAREPUESTO");

                entity.HasOne(d => d.Pago).WithMany(p => p.PagoCompraRepuestos)
                    .HasForeignKey(f => f.IdPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCOMPRA_PAGO");
            });

            modelBuilder.Entity<PagoMantenimiento>(entity =>
            {
                entity.HasKey(e => e.IdPagoMantenimiento).HasName("PRIMARY");

                entity.HasOne(d => d.Mantenimiento).WithMany(p => p.PagoMantenimientos)
                    .HasForeignKey(f => f.IdMantenimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOMANTENIMIENTO_MANTENIMIENTO");

                entity.HasOne(d => d.Pago).WithMany(p => p.PagoMantenimientos)
                    .HasForeignKey(f => f.IdPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOMANTENIMIENTO_PAGO");
            });

            modelBuilder.Entity<PagoCheque>(entity =>
            {
                entity.HasKey(e => e.IdPagoCheque).HasName("PRIMARY");

                entity.HasOne(d => d.Banco).WithMany(p => p.PagoCheques)
                    .HasForeignKey(f => f.IdBanco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOCHEQUE_BANCO");
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
                    .HasForeignKey(f => f.IdLocalidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVEEDOR_LOCALIDAD");
            });

            modelBuilder.Entity<ProveedorEspecialidad>(entity =>
            {
                entity.HasKey(e => e.IdProveedorEspecialidad).HasName("PRIMARY");

                entity.HasOne(d => d.Especialidad).WithMany(p => p.ProveedorEspecialidades)
                    .HasForeignKey(f => f.IdEspecialidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVEEDOR/ESPECIALIDAD_ESPECIALIDAD");

                entity.HasOne(d => d.Proveedor).WithMany(p => p.ProveedorEspecialidades)
                    .HasForeignKey(f => f.IdProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROVEEDOR/ESPECIALIDAD_PROVEEDOR");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.IdProvincia).HasName("PRIMARY");

                entity.Property(e => e.Descripcion).IsFixedLength();
                entity.Property(e => e.UserName).IsFixedLength();

                entity.HasOne(d => d.Pais).WithMany(p => p.Provincias)
                    .HasForeignKey(f => f.IdPais)
                    .HasConstraintName("FK_PROVINCIA_PAIS");
            });

            modelBuilder.Entity<Taller>(entity =>
            {
                entity.HasKey(e => e.IdTaller).HasName("PRIMARY");

                entity.Property(e => e.Mail).IsFixedLength();
                entity.Property(e => e.Nombre).IsFixedLength();
                entity.Property(e => e.Responsable).IsFixedLength();

                entity.HasOne(d => d.Localidad).WithMany(p => p.Talleres)
                    .HasForeignKey(f => f.IdLocalidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALLER_LOCALIDAD");
            });

            modelBuilder.Entity<TallerEspecialidad>(entity =>
            {
                entity.HasKey(e => e.IdTallerEspecialidad).HasName("PRIMARY");

                entity.HasOne(d => d.Especialidad).WithMany(p => p.TallerEspecialidades)
                    .HasForeignKey(f => f.IdEspecialidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALLER/ESPECIALIDAD_ESPECIALIDAD");

                entity.HasOne(d => d.Taller).WithMany(p => p.TallerEspecialidades)
                    .HasForeignKey(f => f.IdTaller)
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
                    .HasForeignKey(f => f.IdCamion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VIAJE_CAMION");

                entity.HasOne(d => d.Chofer).WithMany(p => p.Viajes)
                    .HasForeignKey(f => f.IdChofer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VIAJE_CHOFER");

                entity.HasOne(d => d.Cliente).WithMany(p => p.Viajes)
                    .HasForeignKey(f => f.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VIAJE_CLIENTE");

                entity.HasMany(v => v.Observaciones)
                    .WithOne(o => o.Viaje)
                    .HasForeignKey(f => f.IdViaje);
            });

            modelBuilder.Entity<Destino>(entity =>
            {
                entity.HasKey(e => e.IdDestino);

                entity.HasOne(d => d.Viaje).WithMany(p => p.Destinos)
                    .HasForeignKey(f => f.IdViaje)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Localidad).WithMany(p => p.Destinos)
                    .HasForeignKey(f => f.IdLocalidad)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            
            modelBuilder.Entity<Procedencia>(entity =>
            {
                entity.HasKey(e => e.IdProcedencia);

                entity.HasOne(d => d.Viaje).WithMany(p => p.Procendecias)
                    .HasForeignKey(f => f.IdViaje)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Localidad).WithMany(p => p.Procedencias)
                    .HasForeignKey(f => f.IdLocalidad)
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
        public IDestinoRepo DestinoRepo => new DestinoRepo(this);
        public IProcedenciaRepo ProcedenciaRepo => new ProcedenciaRepo(this);
        public IMantenimientoRepo MantenimientoRepo => new MantenimientoRepo(this);
        public IPagoChequeRepo PagoChequeRepo => new PagoChequeRepo(this);
        public ICobrosRepo CobrosRepo => new CobrosRepo(this);
        public IPagosRepo PagosRepo => new PagosRepo(this);
        public IEspecialidadRepo EspecialidadRepo => new EspecialidadRepo(this);
        public ITallerRepo TallerRepo => new TallerRepo(this);
        #endregion

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
