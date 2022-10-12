using System.ComponentModel.DataAnnotations;

namespace PeliculasApi
{

  public class PostGeneroDTO {
    [Required]
    [StringLength(40)]
    public string Nombre { get; set; }
  }
}