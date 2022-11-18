
using Microsoft.EntityFrameworkCore;

namespace PeliculasApi.Helpers
{

  public static class HttpContextExtensions 
  {
    public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext, IQueryable<T> queryable, int registersByPage)
    {
      double cantidad = await queryable.CountAsync();
      double registers = Math.Ceiling(cantidad / registersByPage);
      httpContext.Response.Headers.Add("CantidadPaginas", registers.ToString());
    }
  }
}