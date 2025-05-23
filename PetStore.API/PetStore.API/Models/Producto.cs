using System.ComponentModel.DataAnnotations;
using PetStore.API.Models;

namespace PetStore.API.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }
    }
}
