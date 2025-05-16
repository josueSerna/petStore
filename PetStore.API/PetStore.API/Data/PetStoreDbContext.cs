using Microsoft.EntityFrameworkCore;
using PetStore.API.Models;

namespace PetStore.API.Data
{
    public class PetStoreDbContext : DbContext
    {
        /// <summary>
        /// Constructor del contexto que recibe opciones de configuración.
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto</param>
        public PetStoreDbContext(DbContextOptions<PetStoreDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<CitaProducto> CitaProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CitaProducto>()
           .HasOne(cp => cp.Cita)
           .WithMany(c => c.CitaProductos)
           .HasForeignKey(cp => cp.CitaId);

            modelBuilder.Entity<CitaProducto>()
                .HasOne(cp => cp.Producto)
                .WithMany()
                .HasForeignKey(cp => cp.ProductoId);
        }
    }
}
