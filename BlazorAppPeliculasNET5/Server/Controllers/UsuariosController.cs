using BlazorAppPeliculasNET5.Server.Helpers;
using BlazorAppPeliculasNET5.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UsuariosController : ControllerBase
	{
		private readonly ApplicationDbContext context;

		public UsuariosController(ApplicationDbContext context)
		{
			this.context = context;
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
	}
}
