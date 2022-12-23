using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GeneroController:ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GeneroController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return Ok(genero);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await context.Generos.ToListAsync();
        }
    }
}
