using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PeliculasApi
{

  [ApiController]
  [Route("api/generos")]
  public class GeneroController : ControllerBase
  {
    private readonly IMapper mapper;
    private readonly ApplicationDbContext context;
    public GeneroController(
      ApplicationDbContext context,
    IMapper mapper
    )
    {
      this.mapper = mapper;
      this.context = context;
    }

    [HttpGet] 
    public async Task<ActionResult<GenericListResponse<GeneroDTO>>> Get()
    {
      try
      {
        var data = await context.Generos.ToListAsync();
        var result = mapper.Map<List<GeneroDTO>>(data);
        return Ok( new GenericListResponse<GeneroDTO>{
          Data = result,
          Message = "Recursos obtenidos correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return NotFound( new GenericListResponse<GeneroDTO>{
          Data = null,
          Message = "Ocurrio un error",
          Status = 400
        });
      }
    }

    [HttpGet("{id:int}", Name = "getGeneroById")]
    public async Task<ActionResult<GenericResponse<GeneroDTO>>> GetById(int id)
    {
      var data = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
      if (data == null) 
      {
        return NotFound( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
      var result = mapper.Map<GeneroDTO>(data);
      return Ok( new GenericResponse<GeneroDTO>{
        Data = result,
        Message = "Recursos obtenidos correctamente",
        Status = 200
      });
    }
  
    [HttpPost]
    public async Task<ActionResult<GenericResponse<GeneroDTO>>> Post([FromBody] PostGeneroDTO generoDTO)
    {
      try
      {
        var data = mapper.Map<Genero>(generoDTO);
        context.Add(data);
        await context.SaveChangesAsync();
        var result = mapper.Map<GeneroDTO>(data);
        return new CreatedAtRouteResult("getGeneroById", new GenericResponse<GeneroDTO>{
          Data = result,
          Message = "Recursos creados correctamente",
          Status = 200,
        });
      }
      catch (System.Exception)
      {
        return BadRequest( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
    }
  
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] PostGeneroDTO generoDTO)
    {
      var data = mapper.Map<Genero>(generoDTO);
      data.Id = id;
      context.Entry(data).State = EntityState.Modified;
      try
      {
        await context.SaveChangesAsync();
        return Ok( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "Recursos actualizados correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return BadRequest( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "Error al actualizar",
          Status = 400
        });
      }
    }
  
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
      var exist = await context.Generos.AnyAsync(x => x.Id == id);

      if (!exist)
      {
        return NotFound( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }

      context.Remove(new Genero() { Id = id });
      await context.SaveChangesAsync();
      return Ok( new GenericResponse<GeneroDTO>{
        Data = null,
        Message = "Recursos actualizados correctamente",
        Status = 200
      });
    }
  }
}