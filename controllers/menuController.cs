using Microsoft.AspNetCore.Mvc;
using backendrestaurante.Models;
using backendrestaurante.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace backendrestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public MenuController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Menu>> GetAll()
        {
            return Ok(_context.Menus.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Menu> GetById(int id)
        {
            var item = _context.Menus.FirstOrDefault(m => m.ItemId == id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Menu> Create([FromBody] MenuItemDto menuDto)
        {
            bool existe = _context.Menus.Any(m => m.Nombre.ToLower() == menuDto.Nombre.ToLower());

            if (existe)
            {
                return BadRequest("Ya existe un ítem del menú con ese nombre.");
            }

            var item = new Menu
            {
                Nombre = menuDto.Nombre,
                Descripcion = menuDto.Descripcion,
                Precio = menuDto.Precio,
                Tipo = menuDto.Tipo
            };

            _context.Menus.Add(item);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MenuItemDto menuDto)
        {
            var item = _context.Menus.FirstOrDefault(m => m.ItemId == id);
            if (item == null)
                return NotFound();

            item.Nombre = menuDto.Nombre;
            item.Descripcion = menuDto.Descripcion;
            item.Precio = menuDto.Precio;
            item.Tipo = menuDto.Tipo;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Menus.FirstOrDefault(m => m.ItemId == id);
            if (item == null)
                return NotFound();

            _context.Menus.Remove(item);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
