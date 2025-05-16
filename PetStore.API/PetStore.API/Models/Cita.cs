using PetStore.API.Models;

namespace PetStore.API.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }

        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }

        public ICollection<CitaProducto> CitaProductos { get; set; }
    }
}
