using ProyectoSeminario.Models.Vehiculo;
using ProyectoSeminario.Models.Vehiculo.ModelsDtos;

namespace ProyectoSeminario.Repository.Vehiculo.IRepository
{
    public interface IRepositoryVehiculo
    {
        ICollection<VehiculoDTO> GetVehiculos();
        ICollection<VehiculoDTO> GetVehiculosPorUsuario(int idUsuario);
        VehiculoDAO GetVehiculo(int vehiculosId);
        bool ExisteVehiculo(int idVehiculo);
        bool CrearVehiculo(CrearVehiculoDTO crearVehiculoDTO);
        bool ActualizarVehiculo(VehiculoDTO vehiculoDTO);
        bool BorrarVehiculo(VehiculoDAO vehiculo);
        bool Guardar();
    }
}