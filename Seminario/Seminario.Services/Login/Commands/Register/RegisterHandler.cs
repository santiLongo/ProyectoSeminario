using System.Net;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Core.ExtensionMethods;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.Login.Commands.Register;

public class RegisterHandler
{
    private readonly IAppDbContext _ctx;

    public RegisterHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(RegisterCommand command)
    {
        var usuario = await _ctx.UsuarioRepo.FindNameAsync(command.Username);

        if (usuario.IsNotNull())
        {
            throw  new SeminarioException("Ya existe un usuario con ese nombre", HttpStatusCode.Conflict);
        }

        if (command.Password != command.ConfirmPassword)
        {
            throw new SeminarioException("Las constrasenas tiene que coincidir", HttpStatusCode.Conflict);
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

        usuario = new Usuario
        {
            Name = command.Username,
            Password = passwordHash,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
        };
        
        _ctx.UsuarioRepo.Add(usuario);
        await _ctx.SaveChangesAsync();
    }
}