using AutoMapper;
using ProyectoSeminario.Models.Usuario;
using ProyectoSeminario.Models.Usuario.ModelsDtos;
using ProyectoSeminario.Models.Vehiculo.ModelsDtos;
using ProyectoSeminario.Models.Vehiculo;

namespace ProyectoSeminario.Mappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<VehiculoDAO, CrearVehiculoDTO>().ReverseMap();
            CreateMap<VehiculoDAO, VehiculoDTO>().ReverseMap();
            CreateMap<UsuarioDAO, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioDAO, CrearUsuarioDTO>().ReverseMap();
            CreateMap<UsuarioDAO, UsuarioLoginTokenDTO>().ReverseMap();
            CreateMap<UsuarioDAO, UsuarioLoginDTO>().ReverseMap();
        }
    }
}