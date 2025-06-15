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
    public class FacturaController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public FacturaController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAll(int page = 1, int pageSize = 10, int? reservaId = null)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Los parámetros de paginación deben ser mayores que cero.");

            var query = _context.Facturas
                .Include(f => f.Reserva)
                    .ThenInclude(r => r.Cliente)
                .Include(f => f.Reserva)
                    .ThenInclude(r => r.Consumos)
                        .ThenInclude(c => c.Item)
                .AsQueryable();

            if (reservaId.HasValue)
            {
                query = query.Where(f => f.ReservaId == reservaId.Value);
            }

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var facturas = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new
                {
                    f.FacturaId,
                    f.ReservaId,
                    f.Fecha,
                    f.Total,
                    Cliente = new
                    {
                        f.Reserva.Cliente.ClienteId,
                        f.Reserva.Cliente.Nombre,
                        f.Reserva.Cliente.Correo,
                        f.Reserva.Cliente.Telefono
                    },
                    Reserva = new
                    {
                        f.Reserva.Fecha,
                        f.Reserva.Estado,
                        f.Reserva.MesaId,
                        f.Reserva.NumeroPersonas
                    },
                    Consumos = f.Reserva.Consumos.Select(c => new
                    {
                        c.ConsumoId,
                        c.ItemId,
                        c.ReservaId,
                        c.Cantidad,
                        c.Fecha
                    }).ToList()
                })
                .ToList();

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = facturas
            };

            return Ok(response);
        }


        [HttpGet("por-reserva/{reservaId}")]
        public ActionResult<IEnumerable<object>> ObtenerPorReserva(int reservaId)
        {
            var consumos = _context.Consumos
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cliente)
                .Include(c => c.Item)
                .Where(c => c.ReservaId == reservaId)
                .Select(c => new
                {
                    c.ConsumoId,
                    c.ReservaId,
                    Cliente = new
                    {
                        c.Reserva.Cliente.ClienteId,
                        c.Reserva.Cliente.Nombre
                    },
                    Item = new
                    {
                        c.ItemId,
                        Nombre = c.Item.Nombre
                    },
                    c.Item.Nombre,
                    c.Item.Precio,
                    c.Cantidad,
                    c.Fecha
                })
                .ToList();

            if (!consumos.Any())
            {
                return NotFound($"No se encontraron consumos para la reserva {reservaId}.");
            }

            return Ok(consumos);
        }




        [HttpGet("{id}")]
        public ActionResult<Factura> GetById(int id)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaId == id);
            if (factura == null)
                return NotFound();

            return Ok(factura);
        }

        [HttpPost]
        public ActionResult<Factura> Create([FromBody] FacturaDto dto)
        {
            var factura = new Factura
            {
                ReservaId = dto.ReservaId,
                Fecha = dto.Fecha ?? DateTime.Now,
                Total = dto.Total
            };

            _context.Facturas.Add(factura);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = factura.FacturaId }, factura);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FacturaDto dto)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaId == id);
            if (factura == null)
                return NotFound();

            factura.ReservaId = dto.ReservaId;
            factura.Fecha = dto.Fecha ?? factura.Fecha;
            factura.Total = dto.Total;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaId == id);
            if (factura == null)
                return NotFound();

            _context.Facturas.Remove(factura);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
