using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IProcedenciaRepo
{
    void RemoveProcedencias(List<Procedencia> procedencias);
    void Add(Procedencia procedencia);
    void Remove(Procedencia procedencia);
}

public class ProcedenciaRepo : IProcedenciaRepo
{
    private readonly AppDbContext _ctx;

    public ProcedenciaRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void RemoveProcedencias(List<Procedencia> procedencias)
    {
        _ctx.Procedencias.RemoveRange(procedencias);
    }

    public void Add(Procedencia procedencia)
    {
        _ctx.Procedencias.Add(procedencia);
    }

    public void Remove(Procedencia procedencia)
    {
        _ctx.Procedencias.Remove(procedencia);
    }
}