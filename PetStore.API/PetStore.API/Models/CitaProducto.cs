using PetStore.API.Models;

namespace PetStore.API.Models
{
    public class CitaProducto
    {
        public int Id { get; set; }

        public int CitaId { get; set; }
        public Cita Cita { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int CantidadUsada { get; set; }
    }
}
