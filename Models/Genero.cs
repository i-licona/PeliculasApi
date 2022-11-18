using System.ComponentModel.DataAnnotations;
using PeliculasApi.Models;

namespace PeliculasApi
{
  public class  Genero{
    public int Id { get; set; }
    [Required]
    [StringLength(40)]
    public string Nombre { get; set; }
    public List<PeliculasGeneros> PeliculasGeneros { get; set; }

  }
}