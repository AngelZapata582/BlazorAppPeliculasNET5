using BlazorAppPeliculasNET5.Server.Helpers;
using BlazorAppPeliculasNET5.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
	public class UsuariosController : ControllerBase
	{
		private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public UsuariosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			this.context = context;
            this.userManager = userManager;
        }

		[HttpGet]
		public async Task<ActionResult<List<UsuarioDTO>>> Get([FromQuery] PaginacionDTO paginacion)
		{
			var queryable = context.Users.AsQueryable();
			await HttpContext.InsertarParametrosPaginacionRespuesta(queryable, paginacion.registros);
			return await queryable.Paginar(paginacion)
				.Select(x => new UsuarioDTO { Id = x.Id, Email = x.Email})
				.ToListAsync();
		}

		[HttpGet("roles")]
		public async Task<ActionResult<List<RolDTO>>> Get()
		{
			return await context.Roles.Select(x=>new RolDTO { Nombre = x.Name}).ToListAsync();
		}

		[HttpPost("asignar")]
		public async Task<ActionResult> AsignarRol(EditarUser editarUser)
		{
            var usuario = await userManager.FindByIdAsync(editarUser.UsuarioId);

            if (usuario == null)
			{
				return BadRequest("No existe el usuario");
			}

			await userManager.AddToRoleAsync(usuario,editarUser.Rol);
			return Ok(usuario);
		}

        [HttpPost("remover")]
        public async Task<ActionResult> RemoverRol(EditarUser editarUser)
        {
            var usuario = await userManager.FindByEmailAsync(editarUser.UsuarioId);

            if (usuario == null)
            {
                return BadRequest("No existe el usuario");
            }

            await userManager.RemoveFromRoleAsync(usuario, editarUser.Rol);
            return Ok(usuario);
        }
    }
}
