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
        [ForeignKey("Mascota")]
        public int IdMascota { get; set; }
        [ForeignKey("Veterinario")]
        public int DNIVeterinario { get; set; }

    }
}
