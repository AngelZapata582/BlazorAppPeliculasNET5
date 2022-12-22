using BlazorAppPeliculasNET5.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActorController:ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ActorController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return Ok(actor);
        }
    }
}
