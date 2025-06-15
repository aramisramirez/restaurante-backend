using Microsoft.AspNetCore.Mvc;
using backendrestaurante.Models;
using backendrestaurante.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ClienteController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAll(int page = 1, int pageSize = 100)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Los parámetros de paginación deben ser mayores que cero.");

            var totalItems = _context.Clientes.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var clientes = _context.Clientes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = clientes
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.ClienteId == id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public ActionResult<Cliente> Create([FromBody] ClienteDto clienteDto)
        {
            var cliente = new Cliente
            {
                Nombre = clienteDto.Nombre,
                Telefono = clienteDto.Telefono,
                Correo = clienteDto.Correo
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = cliente.ClienteId }, cliente);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClienteDto clienteDto)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.ClienteId == id);
            if (cliente == null)
                return NotFound();

            cliente.Nombre = clienteDto.Nombre;
            cliente.Telefono = clienteDto.Telefono;
            cliente.Correo = clienteDto.Correo;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.ClienteId == id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
