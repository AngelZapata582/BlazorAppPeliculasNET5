using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Identity;
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
            

            //configura la entidad
            //con haskey indica que tiene una llave la cual se compone del id de pelicula y genero
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.PeliculaId, x.GeneroId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.PeliculaId, x.ActorId });

            var rolAdmin = new IdentityRole()
            {
                Id = "09da592c-d274-47f8-8e84-58301e1f0013",
                Name = "admin",
                NormalizedName = "admin"
            };
            modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();
        public DbSet<VotoPelicula> VotosPeliculas => Set<VotoPelicula>();
    }
}
