using PetStore.API.Models;

namespace PetStore.API.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public int Edad { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
