using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public VotoController(ApplicationDbContext context,UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Votar(VotoPelicula votoPelicula)
        {
            var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var userId = user.Id;
            var votoActual = await context.VotosPeliculas.FirstOrDefaultAsync( x =>
            x.PeliculaId == votoPelicula.PeliculaId && x.UserId == user.Id);

            if(votoActual == null)
            {
                votoPelicula.UserId = user.Id;
                votoPelicula.FechaVoto = DateTime.Today;
                context.Add(votoPelicula);
                await context.SaveChangesAsync();
            }
            else
            {
                votoActual.Voto = votoPelicula.Voto;
                await context.SaveChangesAsync();
            }
            return Ok(votoPelicula);
        }
    }
}
