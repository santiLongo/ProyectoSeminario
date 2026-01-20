using Microsoft.IdentityModel.Tokens;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Login.Command;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Seminario.Services.Login.Response;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Seminario.Services.Login.Handler
{
    public class AuthHandler
    {
        private readonly IAppDbContext _ctx;
        private readonly IConfiguration _config;
        public AuthHandler(IAppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        public async Task<AuthResponse> Handle(AuthCommand command)
        {
            try
            {
                var usuario = await _ctx.UsuarioRepo.FindNameAsync(command.Mail);

                if (usuario == null)
                {
                    throw new InvalidOperationException("No se encontro el usuario");
                }

                if (!BCrypt.Net.BCrypt.Verify(command.Password, usuario.Password))
                {
                    throw new InvalidOperationException("Contraseña incorrecta");
                }

                //string claveJWT = _config.GetValue<string>("ApiSettings:Secreta");
                string claveJWT = _config.GetSection("ApiSettings:Secreta").Value!;

                var handlerToken = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(claveJWT);
                var expiration = DateTime.UtcNow.AddDays(7);


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, usuario.Role!),
                        new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, usuario.Name!),
                        new Claim(JwtRegisteredClaimNames.Exp,
                            new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString())
                    }),
                    Expires = expiration,
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = handlerToken.CreateToken(tokenDescriptor);

                var response = new AuthResponse
                {
                    Id = usuario.Id,
                    Mail = usuario.Name!,
                    Token = handlerToken.WriteToken(token)
                };

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new InvalidOperationException(e.ToString());
            }
        }
    }
}
