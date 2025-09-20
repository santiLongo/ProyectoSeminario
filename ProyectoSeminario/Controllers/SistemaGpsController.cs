using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;
using ProyectoSeminario.Models.SistemaGps;

namespace ProyectoSeminario.Controllers
{
    [ApiController]
    [Route("SistemaGPS")]
    public class SistemaGPSController : ControllerBase
    {

        private readonly AppDbContext _context;

        public SistemaGPSController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //[Route("nuevaCoordenada")]
        //public ActionResult<SistemaGpsDAO> nuevaCoordenada(string nro_localizador, string latitud, string longitud)
        //{
            

        //    SistemaGpsDAO sistemaGps = _context.Localizadores
        //    .Where(l => l.NroLocalizador == nro_localizador)
        //    .FirstOrDefault();

        //    if (sistemaGps != null)
        //    {
        //        var coordenada = new Coordenada
        //        {
        //            Latitud = latitud,
        //            Longitud = longitud,
        //            IdLocalizador = sistemaGps.Id,
        //        };
                
        //        _context.Coordenadas.Add(coordenada);
        //        _context.SaveChanges();
        //        return Ok(coordenada);
        //    }

        //    return BadRequest("Problema con la coordenada");
        //}
    }
}
