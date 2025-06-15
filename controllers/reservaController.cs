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
    public class ReservaController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ReservaController(RestauranteContext context)
        {
            _context = context;
        }



        [HttpGet]
        public ActionResult GetAll(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Los parámetros de paginación deben ser mayores que cero.");

            var totalItems = _context.Reservas.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var reservas = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new
                {
                    Cliente = r.Cliente.Nombre,
                    Mesa = r.Mesa.Numero,
                    NumeroPersonas = r.NumeroPersonas,
                    Fecha = r.Fecha,
                    ReservaId = r.ReservaId,
                    Estado = r.Estado,
                    MesaId = r.MesaId,
                    ClienteId = r.ClienteId
                })
                .ToList();

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = reservas
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public ActionResult<Reserva> GetById(int id)
        {
            var reserva = _context.Reservas.FirstOrDefault(r => r.ReservaId == id);
            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }

        [HttpPost]
        public ActionResult<Reserva> Create([FromBody] ReservaDto reservaDto)
        {

            if (reservaDto.Fecha < DateTime.Now)
            {
                return BadRequest("No se puede realizar una reserva en una fecha pasada.");
            }


            bool existeReserva = _context.Reservas.Any(r =>
                r.MesaId == reservaDto.MesaId &&
                r.Fecha == reservaDto.Fecha
            );

            if (existeReserva)
            {
                return Conflict("Ya existe una reserva para esta mesa en la misma fecha y hora.");
            }

            var reserva = new Reserva
            {
                ClienteId = reservaDto.ClienteId,
                MesaId = reservaDto.MesaId,
                Fecha = reservaDto.Fecha,
                NumeroPersonas = reservaDto.NumeroPersonas,
                Estado = reservaDto.Estado ?? "Pendiente"
            };

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = reserva.ReservaId }, reserva);
        }



        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ReservaDto reservaDto)
        {
            var reserva = _context.Reservas.FirstOrDefault(r => r.ReservaId == id);
            if (reserva == null)
                return NotFound();

            reserva.ClienteId = reservaDto.ClienteId;
            reserva.MesaId = reservaDto.MesaId;
            reserva.Fecha = reservaDto.Fecha;
            reserva.NumeroPersonas = reservaDto.NumeroPersonas;
            reserva.Estado = reservaDto.Estado ?? reserva.Estado;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reserva = _context.Reservas.FirstOrDefault(r => r.ReservaId == id);
            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
