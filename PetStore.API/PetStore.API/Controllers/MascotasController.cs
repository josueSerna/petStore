using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.API.Data;
using PetStore.API.Models;

namespace PetStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotasController : ControllerBase
    {
        private readonly PetStoreDbContext _context;

        public MascotasController(PetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotas()
        {
            return await _context.Mascotas.Include(m => m.Cliente).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mascota>> GetMascota(int id)
        {
            var mascota = await _context.Mascotas.Include(m => m.Cliente).FirstOrDefaultAsync(m => m.Id == id);
            if (mascota == null) return NotFound();
            return mascota;
        }

        [HttpPost]
        public async Task<ActionResult<Mascota>> PostMascota(Mascota mascota)
        {
            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMascota), new { id = mascota.Id }, mascota);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMascota(int id, Mascota mascota)
        {
            if (id != mascota.Id) return BadRequest();

            var mascotaExistente = await _context.Mascotas.FindAsync(id);
            if (mascotaExistente == null) return NotFound();

            mascotaExistente.Nombre = mascota.Nombre;
            mascotaExistente.Especie = mascota.Especie;
            mascotaExistente.Raza = mascota.Raza;
            mascotaExistente.Edad = mascota.Edad;
            mascotaExistente.ClienteId = mascota.ClienteId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null) return NotFound();

            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
