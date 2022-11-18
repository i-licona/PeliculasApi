
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOS;
using PeliculasApi.Models;
using PeliculasApi.services;

namespace PeliculasApi
{

  [ApiController]
  [Route("api/actores")]
  public class ActorController: ControllerBase {
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IAlmacenarArchivos almacenarArchivos;
    private readonly string contenedor;

    public ActorController(
      ApplicationDbContext context, 
      IMapper mapper,
      IAlmacenarArchivos almacenarArchivos
    ){
      this.context = context;
      this.mapper = mapper;
      this.almacenarArchivos = almacenarArchivos;
      this.contenedor = "actores";
    }

    [HttpGet]
    public async Task<ActionResult<GenericListResponse<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO )
    {
      if (paginacionDTO == null)
      {
        return BadRequest(new GenericListResponse<ActorDTO>{
          Data = null,
          Message = "Envie los parametros de paginación",
          Status = 400
        });
      } 
      else{
        try
        {
          /* calcular total de paginas */
          int totalRegisters = await context.Actores.CountAsync();
          double totalPages = Math.Ceiling(Convert.ToDouble(totalRegisters) / paginacionDTO.RegistersByPage );
          var data = await context.Actores.Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RegistersByPage ).Take(paginacionDTO.RegistersByPage).ToListAsync();
          var result = mapper.Map<List<ActorDTO>>(data);
          return Ok( new GenericListResponse<ActorDTO>{
            Data = result,
            Message = "Recursos obtenidos correctamente",
            Status = 200,
            ActualPage = paginacionDTO.Pagina,
            TotalPages = Convert.ToInt32(totalPages),
            TotalRegisters = totalRegisters          
          });
        }
        catch (System.Exception)
        {
          return BadRequest(new GenericListResponse<ActorDTO>{
            Data = null,
            Message = "Ocurrio un error, intente nuevamente",
            Status = 400
          });
        }  
      }
      
    }

    [HttpGet("{id:int}", Name = "getActorById")]
    public async Task<ActionResult<ActorDTO>> GetById(int id)
    {
      var data = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
      if (data == null)
      {
        return NotFound( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
      var result = mapper.Map<ActorDTO>(data);
      return Ok( new GenericResponse<ActorDTO>{
        Data = result,
        Message = "Recursos obtenidos correctamente",
        Status = 200
      });
    }

    [HttpPost]
    public async Task<ActionResult<GenericResponse<ActorDTO>>> Post([FromForm] PostActorDTO actorDTO){
      var data = mapper.Map<Actor>(actorDTO);
      if (actorDTO.Foto != null)
      {
        using(var memoryStream = new MemoryStream()){
          await actorDTO.Foto.CopyToAsync(memoryStream);
          var contenido = memoryStream.ToArray();
          var extension = Path.GetExtension(actorDTO.Foto.FileName);
          data.Foto = await almacenarArchivos.SaveFile(contenido, extension, contenedor, actorDTO.Foto.ContentType);
        }
      }
      context.Add(data);
     try
     {
       await context.SaveChangesAsync();
       var result = mapper.Map<ActorDTO>(data);
       return new CreatedAtRouteResult("getActorById",new GenericResponse<ActorDTO>{
         Data = result,
         Message = "Recurso creado correctamente",
         Status = 200,
       }); 
     }
     catch (System.Exception)
     {
        return BadRequest(new GenericListResponse<ActorDTO>{
          Data = null,
          Message = "Ocurrio un error, intente nuevamente",
          Status = 400
        });
     }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromForm] PostActorDTO actorDTO)
    {
      var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
      if (actorDB == null)
      {
        return NotFound(new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }
      actorDB = mapper.Map(actorDTO, actorDB);

      if (actorDTO.Foto != null)
      {
        using(var memoryStream = new MemoryStream()){
          await actorDTO.Foto.CopyToAsync(memoryStream);
          var contenido = memoryStream.ToArray();
          var extension = Path.GetExtension(actorDTO.Foto.FileName);
          actorDB.Foto = await almacenarArchivos.EditFile(contenido, extension, contenedor, actorDB.Foto,actorDTO.Foto.ContentType);
        }
      }
      try
      {
        await context.SaveChangesAsync();
        return Ok( new GenericResponse<PostActorDTO>{
          Data = null,
          Message = "Recursos actualizados correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return BadRequest( new GenericResponse<PostActorDTO>{
          Data = null,
          Message = "Error al actualizar",
          Status = 400
        });
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
      var exist = await context.Actores.AnyAsync(x => x.Id == id);

      if (!exist)
      {
        return NotFound( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      }

      context.Remove(new Actor() { Id = id });
      await context.SaveChangesAsync();
      return Ok( new GenericResponse<GeneroDTO>{
        Data = null,
        Message = "Recursos actualizados correctamente",
        Status = 200
      });
    }

    /* [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PatchActorDTO> actorDTO)
    {
      if (actorDTO == null)
      {
        return BadRequest(new GenericResponse<ActorDTO>{
          Data = null,
          Message = "Envie el autor",
          Status = 400
        });
      }
      var dataDb = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
      if (dataDb == null)
      {
        return NotFound( new GenericResponse<GeneroDTO>{
          Data = null,
          Message = "No se encontro el recurso solicitado",
          Status = 400
        });
      } 

      var dataDTO = mapper.Map<PatchActorDTO>(dataDb);
      actorDTO.ApplyTo(dataDTO, ModelState);
      var isValid = TryValidateModel(dataDTO);
      if (!isValid)
      {
        return BadRequest(new GenericListResponse<ActorDTO>{
          Data = ModelState,
          Message = "La información enviada no es correcta",
          Status = 400
        });
      }

      mapper.Map(dataDTO, dataDb);
      await context.SaveChangesAsync();
      return NoContent();
    } */
  }
}