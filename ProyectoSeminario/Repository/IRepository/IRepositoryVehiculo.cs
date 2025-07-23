using ProyectoSeminario.Models;
using ProyectoSeminario.ModelsDtos;

namespace ProyectoSeminario.IRepository
{
    public interface IRepositoryVehiculo
    {
        ICollection<VehiculoDTO> GetVehiculos();
        ICollection<VehiculoDTO> GetVehiculosPorUsuario(int usuarioId);
        VehiculoDTO GetVehiculo(int vehiculosId);
        bool ExisteVehiculo(int id);
        bool CrearVehiculo(VehiculoDAO categoria);
        bool ActualizarVehiculo(VehiculoDAO categoria);
        bool BorrarVehiculo(VehiculoDAO categoria);
        bool Guardar();
    }
}