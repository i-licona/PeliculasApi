using Microsoft.EntityFrameworkCore;

namespace PeliculasApi
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Genero> Generos { get; set; }
  }
}