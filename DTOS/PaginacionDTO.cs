namespace PeliculasApi.DTOS
{
  public class PaginacionDTO 
  {
    public int Pagina { get; set; } 
    private int registersByPage = 10;  
    private readonly int maxregisterByPage = 50;
    public int RegistersByPage 
    {
      get => registersByPage;
      set
      {
        registersByPage = ( value > maxregisterByPage ) ? maxregisterByPage : value;
      }
    }
  }
}