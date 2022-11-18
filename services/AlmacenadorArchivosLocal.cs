namespace PeliculasApi.services
{
  public class AlmacenadorArchivosLocal : IAlmacenarArchivos
  {
    private readonly IWebHostEnvironment env;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AlmacenadorArchivosLocal(
     IWebHostEnvironment env,
     IHttpContextAccessor httpContextAccessor
    )
    {
      this.env = env;
      this.httpContextAccessor = httpContextAccessor;
    }

    public Task DeleteFile(string contenedor, string route)
    {
      if (route != null)
      {
        var FileName = Path.GetFileName(route);
        string directorio = Path.Combine(env.WebRootPath, contenedor, FileName);
        if (File.Exists(directorio))
        {
            File.Delete(directorio);
        }
      }
      return Task.FromResult(0);
    }

    public async Task<string> EditFile(byte[] contenido, string extension, string contenedor, string route, string contentType)
    {
      await DeleteFile(contenedor,route);
      return await SaveFile(contenido, extension,contenedor, contentType);
    }

    public async Task<string> SaveFile(byte[] contenido, string extension, string contenedor, string contentType)
    {
      var FileName = $"{Guid.NewGuid()}{extension}";
      string folder = Path.Combine(env.WebRootPath,contenedor);

      if (!Directory.Exists(folder))
      {
        Directory.CreateDirectory(folder);        
      }
      string route = Path.Combine(folder, FileName);
      await File.WriteAllBytesAsync(route, contenido);
      var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
      var urlParaBd = Path.Combine(urlActual, contenedor, FileName).Replace("\\", "/");
      return urlParaBd;
    }
  }
}