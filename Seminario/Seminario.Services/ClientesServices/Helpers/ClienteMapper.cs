using Seminario.Datos.Entidades;
using Seminario.Datos.ExtensionMethods;
using Seminario.Services.ClientesServices.Models;

namespace Seminario.Services.ClientesServices.Helpers;

public static class ClienteMapper
{
    public static ClienteFormModel MapToForm(this Cliente cliente)
    {
        return new ClienteFormModel
        {
            IdCliente = cliente.IdCliente,
            Cuit = cliente.Cuit.ToLongOrDefault(),
            RazonSocial = cliente.RazonSocial,
            Direccion = cliente.Direccion,
            Telefono = cliente.Telefono,
            Email = cliente.Mail
        };
    }

    public static void MapToEntity(this ClienteFormModel form, Cliente cliente)
    {
        cliente.Direccion = form.Direccion;
        cliente.Telefono = form.Telefono;
        cliente.Mail = form.Email;
    }
}