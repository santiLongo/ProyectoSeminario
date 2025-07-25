using AutoMapper;
using ProyectoSeminario.Repository.IRepository;
using ProyectoSeminario.Mappers;
using ProyectoSeminario.Models;
using ProyectoSeminario.Models.ModelsDtos;
using ProyectoSeminario.Services;

namespace ProyectoSeminario.Repository
{
    public class RepositoryVehiculo : IRepositoryVehiculo
    {

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public RepositoryVehiculo(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public bool ActualizarVehiculo(VehiculoDTO vehiculoDTO)
        {
            var vehiculo = _mapper.Map<VehiculoDAO>(vehiculoDTO);

            _db.Vehiculos.Update(vehiculo);

            return Guardar();
        }

        public bool BorrarVehiculo(VehiculoDAO vehiculo)
        {
            _db.Vehiculos.Remove(vehiculo);

            return Guardar();
        }

        public bool CrearVehiculo(CrearVehiculoDTO crearVehiculoDTO)
        {
            var vehiculo = _mapper.Map<VehiculoDAO>(crearVehiculoDTO);

            _db.Vehiculos.Add(vehiculo);

            return Guardar();
        }

        public VehiculoDAO GetVehiculo(int vehiculosId)
        {
            return _db.Vehiculos.FirstOrDefault(v => v.Id == vehiculosId);
        }

        public bool ExisteVehiculo(int idVehiculo)
        {
            return _db.Vehiculos.Any(v => v.Id == idVehiculo);
        }

        public ICollection<VehiculoDTO> GetVehiculos()
        {
            var vehiculosDto = new List<VehiculoDTO>();

            var listaVehiculos = _db.Vehiculos.ToList();

            foreach (var vehiculo in listaVehiculos)
            {
                vehiculosDto.Add(_mapper.Map<VehiculoDTO>(vehiculo));
            }

            return vehiculosDto;
        }

        public ICollection<VehiculoDTO> GetVehiculosPorUsuario(int idUsuario)
        {
            var vehiculosDto = new List<VehiculoDTO>();

            var listaVehiculos = _db.Vehiculos.Where(v => v.IdUsuario == idUsuario).ToList();

            foreach (var vehiculo in listaVehiculos)
            {
                vehiculosDto.Add(_mapper.Map<VehiculoDTO>(vehiculo));
            }

            return vehiculosDto;
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0? true : false;
        }
    }
}