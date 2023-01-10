using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOS.SalaDeCine;
using PeliculasApi.Models;

namespace PeliculasApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class SalasDeCineController : ControllerBase
  {   
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public SalasDeCineController(
      ApplicationDbContext context,
      IMapper mapper
    )
    {
      this.context = context;
      this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<GenericListResponse<SalaDeCineDTO>>> Get()
    {
      var data = await context.SalasDeCine.ToListAsync(); 
      var result = mapper.Map<List<SalaDeCineDTO>>(data);
      return Ok(new GenericListResponse<SalaDeCineDTO> {
        Data = result,
        Message = "Recursos obtenidos correctamente",
        Status = 200
      });
      // Get<SalaDeCine, SalaDeCineDTO>();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenericResponse<SalaDeCineDTO>>> GetById(int id)
    {
      var data = await context.SalasDeCine.FirstOrDefaultAsync(x => x.Id == id );
      if (data == null)
      {
        return NotFound(new { Message = "No se encontro el recurso" });
      }
      var result = mapper.Map<SalaDeCineDTO>(data);
      return Ok( new GenericResponse<SalaDeCineDTO>{
        Data = result,
        Message = "Recursos obtenidos correctamente",
        Status = 200
      });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] SalaDeCineCreacionDTO salaDeCine)
    {
      var data = mapper.Map<SalaDeCine>(salaDeCine);
      context.SalasDeCine.Add(data);
            
      try
      {
        await context.SaveChangesAsync();
        var result = mapper.Map<SalaDeCineDTO>(data);
        return Ok(new GenericResponse<SalaDeCineDTO>{
          Data	= result,
          Message = "Recursos agregados correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return BadRequest(new { Message = "Verifique la informacion enviada" });
      }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] SalaDeCineCreacionDTO salaDeCine)
    {
      var data = mapper.Map<SalaDeCine>(salaDeCine);
      data.Id = id;
      context.Entry(data).State = EntityState.Modified;
      try
      {
        var result = mapper.Map<SalaDeCineDTO>(data);
        await context.SaveChangesAsync();
        return Ok(new GenericResponse<SalaDeCineDTO>(){
          Data = result,
          Message = "Recursos actualizados correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return BadRequest( new { Message = "Revise los datos enviados" });
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
      var exist = await context.SalasDeCine.AnyAsync( x => x.Id == id);
      if (!exist)
      {
        return NotFound( new { Message = "No se encuentra el recurso solicitado" });
      }
      try
      {
        context.SalasDeCine.Remove( new SalaDeCine(){ Id = id });
        await context.SaveChangesAsync();
        return Ok( new GenericResponse<SalaDeCineDTO>{
          Data = null,
          Message = "Recursos actualizados correctamente",
          Status = 200
        });
      }
      catch (System.Exception)
      {
        return BadRequest( new { Message = "Ocurrio un error, intente nuevamente" });
      }
    }
  }
}