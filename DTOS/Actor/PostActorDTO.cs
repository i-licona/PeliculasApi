using System.ComponentModel.DataAnnotations;
using PeliculasApi.Validaciones;

namespace PeliculasApi
{
    public class PostActorDTO
    {
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [SizeFileValidation(4)]
        [TypeFileValidation(groupType: GroupTypeFile.Imagen)]
        public IFormFile Foto { get; set; }
    }
}