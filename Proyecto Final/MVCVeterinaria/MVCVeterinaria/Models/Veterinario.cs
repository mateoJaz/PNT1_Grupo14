using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCVeterinaria.Models
{
    public class Veterinario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public int DNI { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "La matrícula es obligatoria.")]
        public string Matricula { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public int Telefono { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        public string Especialidad { get; set; }

        public List<Turno> Turnos { get; set; }
    }
}
