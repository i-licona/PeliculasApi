

namespace PeliculasApi
{

 public class GenericListResponse<T> { 
  public List<T> Data { get; set; }
  public string Message { get; set; }
  public int Status { get; set; }
  public int? ActualPage { get; set; } 
  public int? TotalRegisters { get; set; }
  public int? TotalPages { get; set; } 
 }
}