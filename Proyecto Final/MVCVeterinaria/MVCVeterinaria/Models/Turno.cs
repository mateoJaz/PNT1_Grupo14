using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCVeterinaria.Models
{
    public class Turno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? MascotaId { get; set; }
        public Mascota? Mascota { get; set; }
        public int? VeterinarioId { get; set; }
        public Veterinario? Veterinario { get; set; }
        public DateTime FechaHorario { get; set; }
        public String Detalle { get; set; }

    }

}
