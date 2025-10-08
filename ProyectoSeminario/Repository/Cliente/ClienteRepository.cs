using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProyectoSeminario.Services;
using ProyectoSeminario.Entitys;

namespace ProyectoSeminario.Repository.Cliente
{

    public interface IRepositoryCliente
    {
        Task<Entitys.Cliente> FindByIdAsync(int id, bool asNoTracking = false);

        Task<IEnumerable<Entitys.Cliente>> GetAllAsync(bool asNoTracking = false);

        void Add(Entitys.Cliente cliente);

        void RemoveById(Entitys.Cliente cliente);
    }
    public class RepositoryCliente : IRepositoryCliente
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public RepositoryCliente(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Entitys.Cliente> FindByIdAsync(int id, bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.IdCliente == id);
            }
            return await _db.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);
        }

        public async Task<IEnumerable<Entitys.Cliente>> GetAllAsync(bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return await _db.Clientes.AsNoTracking().ToListAsync();
            }

            return await _db.Clientes.ToListAsync();

        }

        public void Add(Entitys.Cliente cliente)
        {
            _db.Clientes.Add(cliente);
        }

        public void RemoveById(Entitys.Cliente cliente)
        {
            _db.Clientes.Remove(cliente);
        }
    }
}
