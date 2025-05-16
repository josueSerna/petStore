using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.API.Data;
using PetStore.API.Models;

namespace PetStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasProductosController : ControllerBase
    {
        private readonly PetStoreDbContext _context;

        public CitasProductosController(PetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaProducto>>> GetCitaProductos()
        {
            return await _context.CitaProductos
                .Include(cp => cp.Producto)
                .Include(cp => cp.Cita)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CitaProducto>> PostCitaProducto(CitaProducto cp)
        {
            _context.CitaProductos.Add(cp);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCitaProductos), new { id = cp.Id }, cp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCitaProducto(int id)
        {
            var cp = await _context.CitaProductos.FindAsync(id);
            if (cp == null) return NotFound();

            _context.CitaProductos.Remove(cp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
