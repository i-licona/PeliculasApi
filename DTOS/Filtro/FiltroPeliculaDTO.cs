
using PeliculasApi.DTOS;

namespace PeliculasApi.DTO.Filtro
{

  public class FiltroPeliculasDTO {
    public int Pagina { get; set; }
    public int RegistersByPage { get; set; }
    public string Titulo { get; set; }
    public int GeneroId { get; set; }
    public bool EnCines { get; set; }
    public bool ProximosEstrenos { get; set; }
    public PaginacionDTO Paginacion {
      get 
      {
        return new PaginacionDTO () 
        { 
          Pagina = Pagina,
          RegistersByPage = RegistersByPage
        };
      } 
    }
  }
}