using Microsoft.AspNetCore.Mvc;
using backendrestaurante.Models;
using backendrestaurante.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace backendrestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesaController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public MesaController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Mesa>> GetAll()
        {
            return Ok(_context.Mesas.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Mesa> GetById(int id)
        {
            var mesa = _context.Mesas.FirstOrDefault(m => m.MesaId == id);
            if (mesa == null)
                return NotFound();

            return Ok(mesa);
        }

        [HttpPost]
        public ActionResult<Mesa> Create([FromBody] MesaDto mesaDto)
        {
            bool existeNumero = _context.Mesas.Any(m => m.Numero == mesaDto.Numero);
            if (existeNumero)
            {
                return BadRequest($"Ya existe una mesa con el número {mesaDto.Numero}.");
            }

            var mesa = new Mesa
            {
                Numero = mesaDto.Numero,
                Capacidad = mesaDto.Capacidad
            };

            _context.Mesas.Add(mesa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = mesa.MesaId }, mesa);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MesaDto mesaDto)
        {
            var mesa = _context.Mesas.FirstOrDefault(m => m.MesaId == id);
            if (mesa == null)
                return NotFound();

            mesa.Numero = mesaDto.Numero;
            mesa.Capacidad = mesaDto.Capacidad;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var mesa = _context.Mesas.FirstOrDefault(m => m.MesaId == id);
            if (mesa == null)
                return NotFound();

            _context.Mesas.Remove(mesa);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
