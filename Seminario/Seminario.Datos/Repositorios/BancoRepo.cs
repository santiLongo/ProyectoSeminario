using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios
{
    public interface IBancoRepo
    {
        Task<IList<Banco>> GetAllAsync(string nombre, bool asNoTracking = false);
        Task<Banco> GetByIdAsync(int id, bool asNoTracking = false);
        void Add(Banco banco);
        void Remove(Banco banco);
    }
    public class BancoRepo : IBancoRepo
    {
        private readonly AppDbContext _ctx;
        public BancoRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IList<Banco>> GetAllAsync(string nombre, bool asNoTracking = false)
        {
            var query = _ctx.Bancos.AsQueryable();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.Where(b => string.IsNullOrEmpty(nombre) || b.Descripcion.Contains(nombre)).ToListAsync();
        }

        public async Task<Banco> GetByIdAsync(int id, bool asNoTracking = false)
        {
            var query = _ctx.Bancos.AsQueryable();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(b => b.IdBanco == id);
        }

        public void Add(Banco banco)
        {
            _ctx.Bancos.Add(banco);
        }

        public void Remove(Banco banco)
        {
            _ctx.Bancos.Remove(banco);
        }
    }
}
