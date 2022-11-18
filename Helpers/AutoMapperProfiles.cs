using AutoMapper;
using PeliculasApi;
using PeliculasApi.DTO;
using PeliculasApi.DTOS.Pelicula;
using PeliculasApi.Models;

namespace PeliculasApi {
  public class AutoMapperProfiles:Profile {
    public AutoMapperProfiles(){
      /* Create mapper Genero */
      CreateMap<Genero, GeneroDTO>().ReverseMap();
      CreateMap<PostGeneroDTO, Genero>();
      /* Create mapper Autor */
      CreateMap<Actor, ActorDTO>().ReverseMap();
      CreateMap<PostActorDTO, Actor>().ForMember(x => x.Foto, options => options.Ignore());
      CreateMap<PatchActorDTO, Actor>().ReverseMap();
      /* Create mapper Peliculas */
      CreateMap<PeliculaDTO, Pelicula>().ReverseMap();
      /* create mapper Peliculas */
      CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
      CreateMap<PeliculaCreacionDTO, Pelicula>().ForMember(x => x.Poster, options => options.Ignore())
      .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapearPeliculasGeneros))
      .ForMember(x => x.PeliculasActores, options => options.MapFrom(MapearPeliculasActores));
      // CreateMap<PatchActorDTO, Actor>().ReverseMap();
    }

    private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
    {
      var result = new List<PeliculasGeneros>();
      if (peliculaCreacionDTO.GenerosIds == null)
      {
        return result;
      }
      foreach(var id in peliculaCreacionDTO.GenerosIds)
      {
        result.Add(new PeliculasGeneros() { GeneroId = id });
      }
      return result;
    }

    private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
    {
      var result = new List<PeliculasActores>();

      if (peliculaCreacionDTO.Actores == null)
      {
        return result;
      }
      
      foreach (var actor in peliculaCreacionDTO.Actores)
      {
        result.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
      }
      return result;
    }
  }
}