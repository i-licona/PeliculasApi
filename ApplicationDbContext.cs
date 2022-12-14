using Microsoft.EntityFrameworkCore;
using PeliculasApi.Models;

namespace PeliculasApi
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Genero> Generos { get; set; }
    public DbSet<Actor> Actores { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<PeliculasActores> PeliculasActores { get; set; }
    public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
    public DbSet<SalaDeCine> SalasDeCine { get; set; }
    public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCine { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<PeliculasActores>().HasKey(x => new { x.ActorId, x.PeliculaId });
      modelBuilder.Entity<PeliculasGeneros>().HasKey(x => new { x.GeneroId, x.PeliculaId });
      modelBuilder.Entity<PeliculasSalasDeCine>().HasKey(x => new { x.PeliculaId, x.SalaDeCineId });
      base.OnModelCreating(modelBuilder);
      
    }
  }
}