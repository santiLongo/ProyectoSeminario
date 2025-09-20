using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;
using ProyectoSeminario.Models;
using ProyectoSeminario.Models.Vehiculo.ModelsDtos;
using ProyectoSeminario.Repository.Vehiculo.IRepository;

namespace ProyectoSeminario.Controllers
{
    [ApiController]
    [Route("api/vehiculos")]
    public class VehiculoController : ControllerBase
    {

        private readonly IRepositoryVehiculo _vehiculoRepo;
        public VehiculoController(IRepositoryVehiculo vehiculoRepo)
        {
            _vehiculoRepo = vehiculoRepo;
        }

        [HttpGet]
        public IActionResult GetVehiculos()
        {
            return Ok(_vehiculoRepo.GetVehiculos());
        }

        [HttpGet("{vehiculoId:int}", Name = "GetVehiculo")]
        public IActionResult GetVehiculo(int idVehiculo)
        {
            var vehiculo = _vehiculoRepo.GetVehiculo(idVehiculo);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return Ok(vehiculo);
        }

        [HttpGet("GetVehiculoPorUsuario")]
        public IActionResult GetVehiculoPorUsuario(int idUsuario)
        {
            return Ok(_vehiculoRepo.GetVehiculosPorUsuario(idUsuario));
        }

        [HttpPost]
        public IActionResult CrearVehiculo([FromBody] CrearVehiculoDTO crearVehiculoDTO)
        {

            if (crearVehiculoDTO == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_vehiculoRepo.CrearVehiculo(crearVehiculoDTO))
            {
                ModelState.AddModelError("", "Error al crear el vehiculo");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

        [HttpDelete("{idVehiculo:int}", Name = "BorrarVehiculo")]
        public IActionResult BorrarVehiculo(int idVehiculo)
        {

            if (!_vehiculoRepo.ExisteVehiculo(idVehiculo))
            {
                return NotFound("El Id del vehiculo no existe.");
            }
            
            var vehiculo = _vehiculoRepo.GetVehiculo(idVehiculo);

            if (!_vehiculoRepo.BorrarVehiculo(vehiculo))
            {
                ModelState.AddModelError("", "Error al borrar el vehiculo");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

        [HttpPatch("ActualizarVehiculo")]
        public IActionResult ActualizarVehiculo([FromBody] VehiculoDTO vehiculoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_vehiculoRepo.ExisteVehiculo(vehiculoDTO.Id))
            {
                return NotFound("El Id del vehiculo no existe.");
            }

            if (!_vehiculoRepo.ActualizarVehiculo(vehiculoDTO))
            {
                ModelState.AddModelError("", "Error al borrar el vehiculo");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }
    }
}