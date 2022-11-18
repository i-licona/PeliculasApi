namespace PeliculasApi.services
{
  public interface IAlmacenarArchivos
  {
    Task<string> SaveFile(byte[] contenido, string extension, string contenedor, string contentType);

    Task<string> EditFile(byte[] contenido, string extension, string contenedor, string route, string contentType);

    Task DeleteFile(string contenedor, string route);
  }  
}