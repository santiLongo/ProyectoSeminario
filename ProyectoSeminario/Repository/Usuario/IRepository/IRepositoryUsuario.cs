using ProyectoSeminario.Models.Usuario;
using ProyectoSeminario.Models.Usuario.ModelsDtos;

namespace ProyectoSeminario.Repository.Usuario.IRepository
{
    public interface IRepositoryUsuario
    {
        UsuarioDAO GetUsuario(int usuarioId);
        bool IsUniqueMail(string mail);
        Task<UsuarioLoginTokenDTO> Login(UsuarioLoginDTO usuarioLoginDTO);
        Task<UsuarioDAO> Registro(CrearUsuarioDTO crearUsuarioDTO);
    }
}
