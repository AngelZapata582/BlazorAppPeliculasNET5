using BlazorAppPeliculasNET5.Server.Helpers;
using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActorController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenamiento almacenamiento;
        private readonly string contenedor = "personas";

        public ActorController(ApplicationDbContext context, IAlmacenamiento almacenador)
        {
            this.context = context;
            this.almacenamiento= almacenador;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Actor actor)
        {
            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actor.Foto = await almacenamiento.GuardarArchivo(fotoActor, ".jpg", contenedor);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return Ok(actor);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get()
        {
            return await context.Actores.ToListAsync();
        }

        [HttpGet("buscar/{texto}")]
        public async Task<ActionResult<List<Actor>>> Get(string texto)
        {
            if(string.IsNullOrWhiteSpace(texto))
            {
                return new List<Actor>();
            }
            return await context.Actores
                .Where(x => x.Nombre.ToLower().Contains(texto.ToLower()))
                .Take(5)
                .ToListAsync();
        }
    }
}
