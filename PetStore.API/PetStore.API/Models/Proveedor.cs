using System.ComponentModel.DataAnnotations;
using PetStore.API.Models;

namespace PetStore.API.Models
{
    public class Proveedor
    {
        public Proveedor()
        {
            Productos = new List<Producto>();
        }

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}
