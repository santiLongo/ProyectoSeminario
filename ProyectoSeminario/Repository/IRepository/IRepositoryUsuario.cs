using ProyectoSeminario.Models;
using ProyectoSeminario.Models.ModelsDtos;

namespace ProyectoSeminario.Repository.IRepository
{
    public interface IRepositoryUsuario
    {
        UsuarioDAO GetUsuario(int usuarioId);
        bool IsUniqueMail(string mail);
        Task<UsuarioLoginTokenDTO> Login(UsuarioLoginDTO usuarioLoginDTO);
        Task<UsuarioDAO> Registro(CrearUsuarioDTO crearUsuarioDTO);
    }
}
