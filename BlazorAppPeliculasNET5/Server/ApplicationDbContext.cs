using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppPeliculasNET5.Server
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configura la entidad
            //con haskey indica que tiene una llave la cual se compone del id de pelicula y genero
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.PeliculaId, x.GeneroId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.PeliculaId, x.ActorId });
        }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();

    }
}
