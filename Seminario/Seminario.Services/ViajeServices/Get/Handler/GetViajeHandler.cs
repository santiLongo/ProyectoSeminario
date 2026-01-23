using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.ViajeServices.Get.Command;
using Seminario.Services.ViajeServices.Get.Model;

namespace Seminario.Services.ViajeServices.Get.Handler;

public class GetViajeHandler
{
    private readonly IAppDbContext _ctx;

    public GetViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GetViajeModel> Handle(GetViajeCommand command)
    {
        var viaje = await _ctx.ViajeRepo.Query()
            .IncludeChofer()
            .IncludeCamion()
            .IncludeCliente()
            .IncludeDestinosProcedencias()
            .FirstOrDefaultAsync(v => v.IdViaje == command.IdViaje);

        if (viaje == null) throw new InvalidOperationException("No se encontro el viaje");

        GetViajeModel model = new();

        model.DatosPrincipales = CargoPrincipales(viaje);
        //
        model.DatosCamion = await CargoCamion(viaje.Camion);
        //
        model.DatosChofer = CargoChofer(viaje.Chofer);
        //
        model.DatosCliente = CargoCliente(viaje.Cliente);
        //
        model.DatosDestino = await CargoDestinos(viaje.Destinos.ToList());
        //
        model.DatosProcedencias = await CargoProcedencias(viaje.Procendecias.ToList());
        
        return model;
    }

    private DatosPrincipales CargoPrincipales(Viaje viaje)
    {
        return new DatosPrincipales
        {
            IdViaje = viaje.IdViaje,
            Kilometros = viaje.Kilometros.GetValueOrDefault(),
            MontoTotal = viaje.MontoTotal,
            PrecioKm = viaje.PrecioKm.GetValueOrDefault(),
            Moneda = "",
            FechaPartida = viaje.FechaPartida,
            FechaDescarga = viaje.FechaDescarga,
            Carga = viaje.Carga,
            Kilos = viaje.Kilos.GetValueOrDefault(),
            UserName = viaje.UserName,
            UserAlta = viaje.UserAlta,
            UserDateTime = viaje.UserDateTime,
            FechaAlta = viaje.FechaAlta
        };
    }
    
    private async Task<DatosCamion> CargoCamion(Camion camion)
    {
        var ultimoMantemiento = await _ctx.MantenimientoRepo.UltimoMantenimientoCamion(camion.IdCamion);
        var tipoCamion = await _ctx.TipoCamionRepo.GetByIdAsync(camion.IdCamion);
        
        return new DatosCamion
        {
            IdCamion = camion.IdCamion,
            Patente = camion.Patente,
            UltimoMantenimiento = ultimoMantemiento?.FechaSalida.GetValueOrDefault(),
            TipoCamion = tipoCamion.Descripcion
        };
    }
    
    private DatosChofer CargoChofer(Chofer chofer)
    {
        return new DatosChofer
        {
            IdChofer = chofer.IdChofer,
            NombreCompleto = $"{chofer.Nombre}  {chofer.Apellido}",
            NroRegistro = chofer.NroRegistro.GetValueOrDefault(),
            Dni = chofer.Dni
        };
    }
    
    private DatosCliente CargoCliente(Cliente cliente)
    {
        return new DatosCliente
        {
            IdCliente = cliente.IdCliente,
            Cuit = cliente.Cuit,
            RazonSocial = cliente.RazonSocial,
        };
    }
    
    private async Task<List<DatosDestino>> CargoDestinos(List<Destino> destinos)
    {
        var lista = new List<DatosDestino>();

        var ids = destinos.Select(d => d.IdLocalidad).ToList();

        var localidades = await _ctx.UbicacionRepo.GetLocalidadRange(ids);
            
        foreach (var destino in localidades)
        {
            var item = new DatosDestino
            {
                IdDestino = destino.IdLocalidad,
                Localidad = destino.Descripcion
            };
            
            lista.Add(item);
        }
        
        return lista;
    }
    
    private async Task<List<DatosProcedencias>> CargoProcedencias(List<Procedencia> procedencias)
    {
        var lista = new List<DatosProcedencias>();

        var ids = procedencias.Select(d => d.IdLocalidad).ToList();

        var localidades = await _ctx.UbicacionRepo.GetLocalidadRange(ids);
            
        foreach (var destino in localidades)
        {
            var item = new DatosProcedencias
            {
                IdProcedencia = destino.IdLocalidad,
                Localidad = destino.Descripcion
            };
            
            lista.Add(item);
        }
        
        return lista;
    }
}