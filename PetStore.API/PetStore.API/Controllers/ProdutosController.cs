using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.API.Data;
using PetStore.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PetStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly PetStoreDbContext _context;

        public ProductosController(PetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.Include(p => p.Proveedor).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.Include(p => p.Proveedor).FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return NotFound();
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            // Validar que el proveedor exista
            var proveedorExistente = await _context.Proveedores.FindAsync(producto.ProveedorId);
            if (proveedorExistente == null)
            {
                return BadRequest("El proveedor especificado no existe.");
            }

            // Evitar que EF intente insertar el objeto Proveedor completo
            producto.Proveedor = null;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
                return BadRequest();

            // Validar que el proveedor exista
            var proveedorExistente = await _context.Proveedores.FindAsync(producto.ProveedorId);
            if (proveedorExistente == null)
            {
                return BadRequest("El proveedor especificado no existe.");
            }

            // Evitar que EF intente actualizar el objeto Proveedor
            producto.Proveedor = null;

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
