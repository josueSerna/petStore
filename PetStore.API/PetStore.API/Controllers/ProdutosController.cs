using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.API.Data;
using PetStore.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.Productos.Include(p => p.Proveedor).OrderBy(p => p.Nombre).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.Include(p => p.Proveedor).FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return NotFound();
            return producto;
        }

        [HttpGet("por-proveedor/{proveedorId}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorProveedor(int proveedorId)
        {
            return await _context.Productos.Where(p => p.ProveedorId == proveedorId).Include(p => p.Proveedor).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var proveedorExistente = await _context.Proveedores.FindAsync(producto.ProveedorId);
            if (proveedorExistente == null)
                return BadRequest("El proveedor especificado no existe.");

            producto.Proveedor = null;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var proveedorExistente = await _context.Proveedores.FindAsync(producto.ProveedorId);
            if (proveedorExistente == null)
                return BadRequest("El proveedor especificado no existe.");

            producto.Proveedor = null;
            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.Id == id)) return NotFound();
                else throw;
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
