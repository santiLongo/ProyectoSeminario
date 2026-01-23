using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IDestinoRepo
{
    void RemoveDestinos(List<Destino> destinos);
    void Add(Destino destino);
    void Remove(Destino destino);
}

public class DestinoRepo : IDestinoRepo
{
    private readonly AppDbContext _ctx;

    public DestinoRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void RemoveDestinos(List<Destino> destinos)
    {
        _ctx.Destinos.RemoveRange(destinos);
    }

    public void Add(Destino destino)
    {
        _ctx.Destinos.Add(destino);
    }

    public void Remove(Destino destino)
    {
        _ctx.Destinos.Remove(destino);
    }
}