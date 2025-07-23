using ProyectoSeminario.Models;
using ProyectoSeminario.ModelsDtos;
using AutoMapper;

namespace ProyectoSeminario.Mappers
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<UsuarioDAO, UsuarioDTO>().ReverseMap();
            CreateMap<VehiculoDAO, VehiculoDTO>().ReverseMap();
        }
    }
}