using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;
using ProyectoSeminario.Models;

namespace ProyectoSeminario.Controllers
{
    [ApiController]
    [Route("vehiculo")]
    public class VehiculoController : ControllerBase
    {
        [HttpGet]
        [Route("obtenerUbicacionActual")]
        public void obtenerUbicacionActual()
        {
            
        }

        [HttpGet]
        [Route("vehiculo")]
        public ActionResult<VehiculoDAO> getVehiculo(int id)
        {
            var context = new AppDb();

            var vehiculo = context.Vehiculos
                            .Where(v => v.Id == id)
                            .Include(v => v.Gps);

            return Ok(vehiculo);  
        }

        [HttpGet]
        [Route("mostrarParadas")]
        public void mostrarParadas()
        {
            
        }
    }
}