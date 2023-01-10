using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.DTOS.SalaDeCine
{
  public class SalaDeCineCreacionDTO 
  {
    [Required]
    [StringLength(120)]
    public string Nombre { get; set; }
  }
}