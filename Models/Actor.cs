
using System.ComponentModel.DataAnnotations;
using PeliculasApi.Models;

namespace PeliculasApi
{

  public class Actor 
  {
    public int Id { get; set; }
    [Required]
    [StringLength(40)]
    public string Nombre { get; set; }
    [Required]
    [StringLength(50)]
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Foto { get; set; }
    public List<PeliculasActores> PeliculasActores { get; set; }
  }
}