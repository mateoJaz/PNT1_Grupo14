using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCVeterinaria.Models
{
    public class Mascota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                var edad = hoy.Year - FechaNacimiento.Year;
                // Restamos un año si aún no cumplió años en la fecha actual
                if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
                return edad;
            }
        }
        public double Peso { get; set; }
        public bool Vivo { get; set; } = true;



        public List<Evento> HistorialClinico { get; set; }

    }
}
