using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IUbicacionRepo
{
    IQueryable<Localidad> LocalidadQuery();
    Task<Localidad> GetLocalidadByIdAsync(int id, bool includeProvincia = false, bool asNoTracking = false);
    Task<IList<Localidad>> GetLocalidadesByProvinciaAsync(int id);
    Task<Provincia> GetProvinciaByLocalidadAsync(int id, bool asNoTracking = false);
    void Add(Localidad localidad);
    void Add(Provincia provincia);
    void Add(Pais pais);
    void Remove(Localidad localidad);
    void Remove(Provincia provincia);
    void Remove(Pais pais);
}

public class UbicacionRepo : IUbicacionRepo
{
    private readonly AppDbContext _ctx;
    
    public UbicacionRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public IQueryable<Localidad> LocalidadQuery()
    {
        return _ctx.Localidades.AsQueryable();
    }
    
    public async Task<Localidad> GetLocalidadByIdAsync(int id, bool includeProvincia = false, bool asNoTracking = false)
    {
        var query = _ctx.Localidades.AsQueryable();

        if (asNoTracking) query = query.AsNoTracking();
        
        if (includeProvincia) query = query.Include(p => p.Provincia);
        
        return await query.FirstOrDefaultAsync(l => l.IdLocalidad == id);
    }

    public async Task<IList<Localidad>> GetLocalidadesByProvinciaAsync(int id)
    {
        return (await _ctx.Provincias.Include(p => p.Localidades).FirstOrDefaultAsync(p => p.IdProvincia == id))
            ?.Localidades.ToList();
    }

    public async Task<Provincia> GetProvinciaByLocalidadAsync(int id, bool asNoTracking = false)
    {
        var query = _ctx.Localidades.AsQueryable();
        
        if (asNoTracking) query = query.AsNoTracking();
        
        return (await query.FirstOrDefaultAsync(l => l.IdLocalidad == id))?.Provincia;
    }

    public void Add(Localidad localidad)
    {
        _ctx.Localidades.Add(localidad);
    }

    public void Add(Provincia provincia)
    {
        _ctx.Provincias.Add(provincia);
    }

    public void Add(Pais pais)
    {
        _ctx.Paises.Add(pais);
    }

    public void Remove(Localidad localidad)
    {
        _ctx.Localidades.Remove(localidad);
    }

    public void Remove(Provincia provincia)
    {
        _ctx.Provincias.Remove(provincia);
    }

    public void Remove(Pais pais)
    {
        _ctx.Paises.Remove(pais);
    }
}

public static class LocalidadQueryExtension
{
    public static IQueryable<Localidad> GetAllLocalidades(this IQueryable<Localidad> query, List<int> idLocalidades) =>
        query.Where(l => idLocalidades.Contains(l.IdLocalidad));
}