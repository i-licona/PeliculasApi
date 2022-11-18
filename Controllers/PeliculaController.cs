
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTO;
using PeliculasApi.DTOS.Pelicula;
using PeliculasApi.Models;
using PeliculasApi.services;

namespace PeliculasApi.Controllers
{


  [ApiController]
  [Route("api/peliculas")]
  public class PeliculaController: ControllerBase
  {
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IAlmacenarArchivos almacenarArchivos;
    private readonly string contenedor = "peliculas";

    public PeliculaController(
      ApplicationDbContext context,
      IMapper mapper,
      IAlmacenarArchivos almacenarArchivos
    )
    {
      this.context = context;
      this.mapper = mapper;
      this.almacenarArchivos = almacenarArchivos;
    }

    [HttpGet]
    public async Task<ActionResult<GenericListResponse<PeliculaDTO>>> Get()
    {
      var data = await context.Peliculas.ToListAsync();
      var result = mapper.Map<List<PeliculaDTO>>(data);
      return Ok( new GenericListResponse<PeliculaDTO>{
        Data = result,
        Message = "Recursos obtenidos correctamente",
        Status = 200
      });
    }

    [HttpGet("{id:int}", Name = "getPeliculaById")]
    public async Task<ActionResult<GenericResponse<PeliculaDTO>>> GetById(int id)
    {
      var data = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
      if (data == null)
      {
         return NotFound( new GenericResponse<PeliculaDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
      var result = mapper.Map<PeliculaDTO>(data);        
      return Ok( new GenericResponse<PeliculaDTO>{
      Data = result,
      Message = "Recursos obtenidos correctamente",
      Status = 200
      });
    }

    [HttpPost]
    public async Task<ActionResult<GenericResponse<PeliculaDTO>>> Post([FromForm] PeliculaCreacionDTO pelicula)
    {
      var data = mapper.Map<Pelicula>(pelicula);

      if (pelicula.Poster != null)
      {
        using(var memoryStream = new MemoryStream()){
          await pelicula.Poster.CopyToAsync(memoryStream);
          var contenido = memoryStream.ToArray();
          var extension = Path.GetExtension(pelicula.Poster.FileName);
          data.Poster = await almacenarArchivos.SaveFile(contenido, extension, contenedor, pelicula.Poster.ContentType);
        }
      }
      context.Add(data);
      try
      {
        AsignarOrdenActores(data);
        await context.SaveChangesAsync();
        var result = mapper.Map<PeliculaDTO>(data);
        return new CreatedAtRouteResult("getPeliculaById",new GenericResponse<PeliculaDTO>{
          Data = result,
          Message = "Recurso creado correctamente",
          Status = 200,
        }); 
      }
      catch (System.Exception)
      {
        return BadRequest(new GenericListResponse<PeliculaDTO>{
            Data = null,
            Message = "Ocurrio un error, intente nuevamente",
            Status = 400
          });
      }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenericResponse<PeliculaDTO>>> Put(int id,[FromForm] PeliculaCreacionDTO peliculaDTO)
    { 
      var peliculaDB = await context.Peliculas
      .Include(x => x.PeliculasActores)
      .Include(x => x.PeliculasGeneros)
      .FirstOrDefaultAsync(x => x.Id == id);
      if (peliculaDB == null)
      {
        return NotFound(new GenericResponse<PeliculaDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
      peliculaDB = mapper.Map(peliculaDTO, peliculaDB);

      if (peliculaDTO.Poster != null)
      {
        using(var memoryStream = new MemoryStream()){
          await peliculaDTO.Poster.CopyToAsync(memoryStream);
          var contenido = memoryStream.ToArray();
          var extension = Path.GetExtension(peliculaDTO.Poster.FileName);
          peliculaDB.Poster = await almacenarArchivos.EditFile(contenido, extension, contenedor, peliculaDB.Poster,peliculaDTO.Poster.ContentType);
        }
      }
      try
      {
        var peliculaUpdate = mapper.Map<PeliculaDTO>(peliculaDB);
        AsignarOrdenActores(peliculaDB);
        await context.SaveChangesAsync();
        return Ok( new GenericResponse<PeliculaDTO>{
          Data = peliculaUpdate,
          Message = "Recursos actualizados correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return BadRequest( new GenericResponse<PeliculaDTO>{
          Data = null,
          Message = "Error al actualizar",
          Status = 400
        });
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id){
      var exist = await context.Peliculas.AnyAsync(x => x.Id == id);
      if (!exist)
      {
          return NotFound(new GenericResponse<Pelicula>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
      context.Remove(new Pelicula() { Id = id });
      await context.SaveChangesAsync();
      return Ok( new GenericResponse<Pelicula>{
        Data = null,
        Message = "Recursos actualizados correctamente",
        Status = 200
      });
    }

    private void AsignarOrdenActores(Pelicula pelicula)
    {
      if (pelicula.PeliculasActores != null)
      {
        for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
        {
          pelicula.PeliculasActores[i].Orden = i;
        }
      }
    }

  }
}