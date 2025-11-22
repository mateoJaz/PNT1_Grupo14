using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCVeterinaria.Models
{
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TipoEvento { get; set; }
        public String Detalle { get; set; }
        public DateTime FechaHorario { get; set; }
        public int MascotaId { get; set; }
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }

    }
}
