using AutoMapper;
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
        private readonly IMapper mapper;
        private readonly string contenedor = "peliculas";
        private readonly string extencion = ".jpg";

        public PeliculasController(ApplicationDbContext context, IAlmacenamiento almacenamiento, IMapper mapper)
        {
            this.context = context;
            this.almacenamiento = almacenamiento;
            this.mapper = mapper;
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
                pelicula.Poster = await almacenamiento.GuardarArchivo(fotoActor, extencion, contenedor);
            }

            OrdenarActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return Ok(pelicula);
        }

        private static void OrdenarActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActor is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }
        }

        [HttpGet("actualizar/{id}")]
        public async Task<ActionResult<UpdatePeliculaDTO>> PutGet(int id)
        {
            //usa el metodo de la misma clase
            var peliculaResult = await Get(id);
            if (peliculaResult.Result is NotFoundResult) { return NotFound(); }
            var pelicula = peliculaResult.Value;
            var generosSeleccionadosIds = pelicula.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos.Where(x => !generosSeleccionadosIds.Contains(x.Id)).ToListAsync();

            var modelo = new UpdatePeliculaDTO();
            modelo.Pelicula = pelicula.Pelicula;
            modelo.GenerosNoSeleccionados = generosNoSeleccionados;
            modelo.GenerosSeleccionados = pelicula.Generos;
            modelo.Actores = pelicula.Actores;
            return modelo;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Pelicula pelicula)
        {
            var peliculaDb = await context.Peliculas
                .Include(x => x.GeneroPeliculas)
                .Include(x => x.PeliculasActor)
                .FirstOrDefaultAsync(x => x.Id == pelicula.Id);
            if(peliculaDb is null)
            {
                return NotFound();
            }

            peliculaDb = mapper.Map(pelicula, peliculaDb);

            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var poster = Convert.FromBase64String(pelicula.Poster);
                peliculaDb.Poster = await almacenamiento.EditarArchivo(poster, extencion, contenedor, peliculaDb.Poster);
            }
            OrdenarActores(peliculaDb);
            await context.SaveChangesAsync();
            return Ok(peliculaDb);
        }
    }
}
