using BlazorAppPeliculasNET5.Server.Helpers;
using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var fotoActor = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenamiento.GuardarArchivo(fotoActor, ".jpg", contenedor);
            }
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return Ok(pelicula);
        }
    }
}
