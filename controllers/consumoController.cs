using Microsoft.AspNetCore.Mvc;
using backendrestaurante.Models;
using backendrestaurante.Dtos;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace backendrestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumoController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ConsumoController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetAll(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Los parámetros de paginación deben ser mayores que cero.");

            var totalItems = _context.Consumos.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var consumos = _context.Consumos
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cliente)
                .Include(c => c.Item)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    ReservaId = c.ReservaId,
                    ClienteId = c.Reserva.ClienteId,
                    NombreCliente = c.Reserva.Cliente.Nombre,
                    ItemId = c.ItemId,
                    NombreMenu = c.Item.Nombre,
                    Cantidad = c.Cantidad,
                    Fecha = c.Fecha,
                    ConsumoId = c.ConsumoId,
                })
                .ToList();

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = consumos
            };

            return Ok(response);
        }




        [HttpGet("{id}")]
        public ActionResult<Consumo> GetById(int id)
        {
            var consumo = _context.Consumos.FirstOrDefault(c => c.ConsumoId == id);
            if (consumo == null)
                return NotFound();

            return Ok(consumo);
        }

        [HttpPost]
        public ActionResult<Consumo> Create([FromBody] ConsumoDto dto)
        {
            var consumo = new Consumo
            {
                ReservaId = dto.ReservaId,
                ItemId = dto.ItemId,
                Cantidad = dto.Cantidad,
                Fecha = dto.Fecha
            };

            _context.Consumos.Add(consumo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = consumo.ConsumoId }, consumo);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ConsumoDto dto)
        {
            var consumo = _context.Consumos.FirstOrDefault(c => c.ConsumoId == id);
            if (consumo == null)
                return NotFound();

            consumo.ReservaId = dto.ReservaId;
            consumo.ItemId = dto.ItemId;
            consumo.Cantidad = dto.Cantidad;
            consumo.Fecha = dto.Fecha;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var consumo = _context.Consumos.FirstOrDefault(c => c.ConsumoId == id);
            if (consumo == null)
                return NotFound();

            _context.Consumos.Remove(consumo);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
