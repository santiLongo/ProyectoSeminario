using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;
using ProyectoSeminario.Models;
using ProyectoSeminario.ModelsDtos;

namespace ProyectoSeminario.Controllers
{
    [ApiController]
    [Route("inicio")]
    public class UsuarioController : ControllerBase
    {

        [HttpGet]
        [Route("iniciarSesion")]
        public ActionResult<UsuarioDTO> iniciarSesion(string mail, string password)
        {
            using var context = new AppDb();

            var usuarioDb = context.Usuarios
                .Include(u => u.UserVehiculos)
                .FirstOrDefault(u => u.Mail == mail);

            if (usuarioDb != null && BCrypt.Net.BCrypt.Verify(password, usuarioDb.Password))
            {
                var user = new UsuarioDTO
                {
                    Id = usuarioDb.Id,
                    Mail = usuarioDb.Mail,
                };

                foreach (var v in usuarioDb.UserVehiculos)
                {
                    user.Vehiculos.Add(new VehiculoDTO
                    {
                        Id = v.Id,
                        Patente = v.Patente,
                        Marca = v.Marca,
                        Modelo = v.Modelo,
                    });
                }
                return Ok(user);
            }

            return Unauthorized("Credenciales inválidas");
        }

        [HttpPost]
        [Route("registrarUsuario")]
        public async Task<IActionResult> registrarUsuario(string mail, string password)
        {
            using var context = new AppDb();

            if (context.Usuarios.Any(u => u.Mail != mail))
            {
                var usuario = new UsuarioDAO(mail, BCrypt.Net.BCrypt.HashPassword(password));
                context.Usuarios.Add(usuario);
                context.SaveChanges();
                return Ok(usuario);
            }
            
            return BadRequest("Ya existe un usuario con ese mail.");
            
        }
    }
}