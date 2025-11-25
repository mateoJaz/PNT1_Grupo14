using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MVCVeterinaria.Models
{
    public class Cliente
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
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public int Telefono { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }

        public List<Mascota> Mascotas { get; set; }
        public List<Turno> Turnos { get; set; }

        public String NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }

    }
}
