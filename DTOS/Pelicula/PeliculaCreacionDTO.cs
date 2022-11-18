
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PeliculasApi.Helpers;
using PeliculasApi.Validaciones;

namespace PeliculasApi.DTOS.Pelicula
{

  public class PeliculaCreacionDTO 
  {
    [Required]
    [StringLength(300)]
    public string Titulo { get; set; }
    public bool   EnCines { get; set; }
    public DateTime FechaEstreno { get; set; }
    [SizeFileValidation(4)]
    [TypeFileValidation(groupType: GroupTypeFile.Imagen)]
    public IFormFile Poster { get; set; }
    [ModelBinder(binderType: typeof(TypeBinder<List<int>>))]
    public List<int> GenerosIds { get; set; }
    [ModelBinder(binderType: typeof(TypeBinder<List<ActorPeliculasCreacionDTO>>))]
    public List<ActorPeliculasCreacionDTO>  Actores { get; set; } 
  }
}