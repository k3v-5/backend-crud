using System.Text.Json.Serialization;
namespace backend_crud.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int SalaDeJuntasId { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }

        
        public SalaDeJuntas? SalaDeJuntas { get; set; }
    }
}