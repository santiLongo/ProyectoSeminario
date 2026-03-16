using Seminario.Datos.Contextos.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminario.Services.ViajeServices.GetObservaciones
{
    public class ViajeGetObservacionesHandler
    {
        private readonly IAppDbContext _ctx;

        public ViajeGetObservacionesHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<ViajeGetObservacionesResponse>> Handle(ViajeGetObservacionesCommand command)
        {
            var observaciones = await _ctx.ViajeRepo.FindObsByIdAsync(command.IdViaje);

            return observaciones.Select(o => new ViajeGetObservacionesResponse
            {
                Observacion = o.Observacion,
                UserDateTime = o.UserDateTime,
                UserName = o.UserName,
            });
        }
    }
}
