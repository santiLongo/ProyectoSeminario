using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;
using ProyectoSeminario.Models;
using ProyectoSeminario.Models.ModelsDtos;
using ProyectoSeminario.Repository.IRepository;

namespace ProyectoSeminario.Controllers
{
    [ApiController]
    [Route("api/inicio")]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepositoryUsuario _usuarioRepo;
        public UsuarioController(IRepositoryUsuario usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [HttpPost("iniciarSesion")]
        public async Task<IActionResult> iniciarSesion([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            if (usuarioLoginDTO.Mail == null && usuarioLoginDTO.Password == null)
            {
                return BadRequest("Ninguno de los campos debe estar vacio");
            }

            var usuario = await _usuarioRepo.Login(usuarioLoginDTO);

            if(usuario != null)
            {
                return Ok(usuario);
            }

            return Unauthorized("Credenciales inválidas");
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] CrearUsuarioDTO crearUsuarioDTO)
        {

            if(crearUsuarioDTO.Mail == null && crearUsuarioDTO.Password == null)
            {
                return BadRequest("Ninguno de los campos debe estar vacio");
            }

            if (_usuarioRepo.IsUniqueMail(crearUsuarioDTO.Mail))
            {
                return BadRequest("Ya existe un usuario con ese mail.");
            }

            return Ok(await _usuarioRepo.Registro(crearUsuarioDTO));
            
        }
    }
}