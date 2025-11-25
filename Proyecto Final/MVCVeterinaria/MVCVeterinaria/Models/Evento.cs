using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCVeterinaria.Models
{
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El tipo de evento es obligatorio.")]
        public string TipoEvento { get; set; }
        [Required(ErrorMessage = "El detalle o diagnóstico es obligatorio.")]
        public String Detalle { get; set; }
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime FechaHorario { get; set; }
        public int MascotaId { get; set; }
        [ForeignKey("MascotaId")]
        public Mascota Mascota { get; set; }
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }

    }
}
