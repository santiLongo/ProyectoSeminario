using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using ProyectoSeminario.Models;
using ProyectoSeminario.Models.ModelsDtos;
using ProyectoSeminario.Repository.IRepository;
using ProyectoSeminario.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ProyectoSeminario.Repository
{
    public class RepositoryUsuario : IRepositoryUsuario
    {

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private string claveJWT;

        public RepositoryUsuario(AppDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            claveJWT = config.GetValue<string>("ApiSettings:Secreta");
        }

        public UsuarioDAO GetUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueMail(string mail)
        {
            return _db.Usuarios.Any(u => u.Mail == mail);
        }

        public async Task<UsuarioLoginTokenDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var usuario = _db.Usuarios
                .FirstOrDefault(
                u => u.Mail == usuarioLoginDTO.Mail);

            if(usuario == null || !BCrypt.Net.BCrypt.Verify(usuarioLoginDTO.Password, usuario.Password))
            {
                return new UsuarioLoginTokenDTO
                {
                    Usuario = null,
                    Token = ""
                };
            }

            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveJWT);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadoToken.CreateToken(tokenDescriptor);

            var usuarioLoginTokenDTO = new UsuarioLoginTokenDTO
            {
                Usuario = _mapper.Map<UsuarioDTO>(usuario),
                Token = manejadoToken.WriteToken(token)
            };

            return usuarioLoginTokenDTO;
        }

        public async Task<UsuarioDAO> Registro(CrearUsuarioDTO crearUsuarioDTO)
        {
            var passwordEncriptada = BCrypt.Net.BCrypt.HashPassword(crearUsuarioDTO.Password);

            var usuario = new UsuarioDAO
            {
                Mail = crearUsuarioDTO.Mail,
                Password = passwordEncriptada, 
                Role = crearUsuarioDTO.Role
            };

            _db.Add(usuario);
            await _db.SaveChangesAsync();
            return usuario;

        }
    }
}
