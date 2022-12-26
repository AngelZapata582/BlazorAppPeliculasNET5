using BlazorAppPeliculasNET5.Server.Helpers;
using BlazorAppPeliculasNET5.Shared.DTOs;
using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenamiento almacenamiento;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IAlmacenamiento almacenamiento)
        {
            this.context = context;
            this.almacenamiento = almacenamiento;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limit = 6;
            var peliculasEnCartelera = await context.Peliculas
                .Where(pelicula => pelicula.EnCartelera).Take(limit)
                .OrderByDescending(pelicula => pelicula.Lanzamiento)
                .ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await context.Peliculas
                .Where(pelicula => pelicula.Lanzamiento > fechaActual)
                .OrderBy(pelicula => pelicula.Lanzamiento).Take(limit)
                .ToListAsync();

            var resultado = new HomePageDTO
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                PeliculasDeEstreno = proximosEstrenos
            };

            return resultado;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VisualizarPeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.Where(pelicula => pelicula.Id == id)
                .Include(pelicula => pelicula.GeneroPeliculas)
                .ThenInclude(gp => gp.Genero)
                .Include(pelicula => pelicula.PeliculasActor.OrderBy(ap => ap.Orden))
                .ThenInclude(ap => ap.Actor)
                .FirstOrDefaultAsync();
            if(pelicula is null)
            {
                return NotFound();
            }
            //TODO: sistema de voto
            var promedio = 4;
            var voto = 5;

            var modelo = new VisualizarPeliculaDTO();
            modelo.Pelicula = pelicula;
            modelo.Generos = pelicula.GeneroPeliculas.Select(gp => gp.Genero).ToList();
            modelo.Actores = pelicula.PeliculasActor.Select(ap => new Actor
            {
                Nombre = ap.Actor.Nombre,
                Foto = ap.Actor.Foto,
                Personaje = ap.Personaje,
                Id = ap.ActorId
            }).ToList();
            modelo.Promedio = promedio;
            modelo.Voto = voto;
            return modelo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var fotoActor = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenamiento.GuardarArchivo(fotoActor, ".jpg", contenedor);
            }

            if(pelicula.PeliculasActor is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return Ok(pelicula);
        }
    }
}
