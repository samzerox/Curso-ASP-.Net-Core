using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using contactos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contactos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactosContext _context;

        public ContactController(ContactosContext context)
        {
            _context = context;
        }

        // Devuelve todos los datos
        [HttpGet]
        public IEnumerable<Contacto> getAll()
        {
            return _context.Contacto.ToList();
        }


        // Devuelve un rgistro por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Contacto>> GetById(long id)
        {
            var item = await _context.Contacto.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }


        // Metodo Post
        [HttpPost]
        public async Task<ActionResult<Contacto>> Create([FromBody] Contacto item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Contacto.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = item.id}, item);
        }

        // Metodo PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Contacto item)
        {
            if (item == null || id==0)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Metodo Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var contact = await _context.Contacto.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacto.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}