using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/generos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await context.Generos.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Id == id);
            if(genero is null)
            {
                return NotFound();
            }
            return Ok(genero);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genero genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
            return Ok(genero);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filasAfectadas = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (filasAfectadas is null)
            {
                return NotFound(id);
            }
            
            context.Remove(filasAfectadas);
            await context.SaveChangesAsync();
            return Ok(filasAfectadas);
        }
    }
}
