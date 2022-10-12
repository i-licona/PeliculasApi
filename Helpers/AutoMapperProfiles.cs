using AutoMapper;

namespace PeliculasApi {
  public class AutoMapperProfiles:Profile {
    public AutoMapperProfiles(){
      /* Create mapper */
      CreateMap<Genero, GeneroDTO>().ReverseMap();
      CreateMap<PostGeneroDTO, Genero>();
    }
  }
}