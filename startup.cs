using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PeliculasApi.services;

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
      services.AddAutoMapper(typeof(Startup));
      /* configurar servicio de subida de archivos en Azure */
      /* services.AddTransient<IAlmacenarArchivos, AlmacenadorArchivosAzure>();  */     
      /* configurar servicio de subida de archivos en Local */
      services.AddTransient<IAlmacenarArchivos, AlmacenadorArchivosLocal>();
      services.AddHttpContextAccessor(); 
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        this.configuration.GetConnectionString("DevConnection")
      ));
      services.AddControllers().AddNewtonsoftJson();
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
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}