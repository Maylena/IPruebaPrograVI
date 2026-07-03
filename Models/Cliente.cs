using Microsoft.AspNetCore.Routing.Constraints;

namespace IPruebaPrograVI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
