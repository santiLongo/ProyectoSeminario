using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios
{
    public interface IUsuarioRepo
    {
        void Add(Usuario usuario);
        void Remove(Usuario usuario);
        Task<Usuario> FindNameAsync(string mail, bool asNoTracking = false);
    }
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly AppDbContext _ctx;

        public UsuarioRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Add(Usuario usuario)
        {
            _ctx.Usuarios.Add(usuario);
        }

        public void Remove(Usuario usuario)
        {
            _ctx.Usuarios.Remove(usuario);
        }

        public async Task<Usuario> FindNameAsync(string name, bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return await _ctx.Usuarios.AsNoTracking().FirstOrDefaultAsync(e => e.Name == name);
            }
            return await _ctx.Usuarios.FirstOrDefaultAsync(e => e.Name == name);
        }
    }
}
