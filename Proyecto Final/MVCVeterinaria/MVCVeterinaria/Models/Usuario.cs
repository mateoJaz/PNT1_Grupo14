using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCVeterinaria.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }

        public string? Nombre { get; set; }
    }
}
