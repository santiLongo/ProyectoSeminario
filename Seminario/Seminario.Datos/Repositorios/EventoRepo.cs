using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IEventoRepo
{
    void Add(Evento evento);
    void Add(TipoEvento tipo);
    Task Desactivar(Evento evento);
    Task Activar(Evento evento);

    Task<Evento> FindByIdAsync(int id, bool asNoTracking = false, bool includeTipo = false);
    Task<TipoEvento> FindTipoByIdAsync(int id);
    Task<IEnumerable<Evento>> FindByTipoAsync(int idTipo, bool asNoTracking = false);
    Task<IEnumerable<TipoEvento>> GetAllTipos();
    
}

public class EventoRepo : IEventoRepo
{
    private readonly AppDbContext _ctx;
    
    public EventoRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void Add(Evento evento)
    {
        _ctx.Eventos.Add(evento);
    }
    
    public void Add(TipoEvento tipo)
    {
        _ctx.TiposEvento.Add(tipo);
    }

    public async Task Desactivar(Evento evento)
    {
        evento.Inactivo = true;
        await _ctx.SaveChangesAsync();
    }

    public async Task Activar(Evento evento)
    {
        evento.Inactivo = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task<Evento> FindByIdAsync(int id, bool asNoTracking = false, bool includeTipo = false)
    {
        var query = _ctx.Eventos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();
        
        if (includeTipo)
            query = query.Include(t => t.TipoEvento);

        return await query.FirstOrDefaultAsync(e => e.IdEvento == id);
    }

    public async Task<TipoEvento> FindTipoByIdAsync(int id)
    {
        return await _ctx.TiposEvento.FirstOrDefaultAsync(t => t.IdTipo == id);
    }

    public async Task<IEnumerable<Evento>> FindByTipoAsync(int idTipo, bool asNoTracking = false)
    {
        return await _ctx.Eventos.Where(e => e.IdEvento == idTipo).ToListAsync();
    }

    public async Task<IEnumerable<TipoEvento>> GetAllTipos()
    {
        return await _ctx.TiposEvento.ToListAsync();
    }
}