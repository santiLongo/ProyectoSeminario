using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;
using ProyectoSeminario.Models;

namespace ProyectoSeminario.Controllers
{
    [ApiController]
    [Route("SistemaGPS")]
    public class SistemaGPSController : ControllerBase
    {

        [HttpGet]
        [Route("nuevaCoordenada")]
        public ActionResult<SistemaGpsDAO> nuevaCoordenada(string nro_localizador, string latitud, string longitud)
        {
            var context = new AppDb();

            SistemaGpsDAO sistemaGps = context.Localizadores
            .Where(l => l.NroLocalizador == nro_localizador)
            .FirstOrDefault();

            if (sistemaGps != null)
            {
                var coordenada = new Coordenada
                {
                    Latitud = latitud,
                    Longitud = longitud,
                    IdLocalizador = sistemaGps.Id,
                };

                sistemaGps.agregarCoordenada(coordenada);

                return Ok(sistemaGps);
            }

            return BadRequest("Problema con la coordenada");
        }
    }
}