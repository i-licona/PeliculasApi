using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace PeliculasApi
{
  public class Startup
  {
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    /* Configure services method */
    public void ConfigureServices(IServiceCollection services){
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        this.configuration.GetConnectionString("DevConnection")
      ));
      services.AddControllers();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen( s => {
        s.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Peliculas", Version = "v1"});
      });
    }

    /* Configure method */
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}