using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.API.Data;
using PetStore.API.Models;

namespace PetStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly PetStoreDbContext _context;

        public ClienteController(PetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.Include(c => c.Mascotas).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.Include(c => c.Mascotas).FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null) return NotFound();
            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest();

            var clienteExistente = await _context.Clientes
                .Include(c => c.Mascotas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clienteExistente == null)
                return NotFound();

            // Actualizar campos simples del cliente
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Email = cliente.Email;

            clienteExistente.Mascotas ??= new List<Mascota>();

            var mascotasEnviadasIds = cliente.Mascotas?.Select(m => m.Id).ToList() ?? new List<int>();

            // Eliminar mascotas que ya no están en la lista enviada
            var mascotasParaEliminar = clienteExistente.Mascotas
                .Where(m => !mascotasEnviadasIds.Contains(m.Id))
                .ToList();

            foreach (var mascota in mascotasParaEliminar)
            {
                _context.Mascotas.Remove(mascota);
            }

            // Actualizar o agregar mascotas enviadas
            foreach (var mascotaEnviada in cliente.Mascotas ?? new List<Mascota>())
            {
                var mascotaExistente = clienteExistente.Mascotas
                    .FirstOrDefault(m => m.Id == mascotaEnviada.Id);

                if (mascotaExistente != null)
                {
                    // Actualizar mascota existente
                    mascotaExistente.Nombre = mascotaEnviada.Nombre;
                    mascotaExistente.Especie = mascotaEnviada.Especie;
                    mascotaExistente.Raza = mascotaEnviada.Raza;
                    mascotaExistente.Edad = mascotaEnviada.Edad;
                }
                else
                {
                    // Agregar nueva mascota
                    clienteExistente.Mascotas.Add(new Mascota
                    {
                        Nombre = mascotaEnviada.Nombre,
                        Especie = mascotaEnviada.Especie,
                        Raza = mascotaEnviada.Raza,
                        Edad = mascotaEnviada.Edad,
                        ClienteId = clienteExistente.Id
                    });
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
