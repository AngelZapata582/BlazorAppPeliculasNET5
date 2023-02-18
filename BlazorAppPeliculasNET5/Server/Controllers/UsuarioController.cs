using BlazorAppPeliculasNET5.Client.Shared;
using BlazorAppPeliculasNET5.Server.Helpers;
using BlazorAppPeliculasNET5.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsuarioController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionRespuesta(queryable, paginacion.registros);
            return await queryable.Paginar(paginacion)
                .Select(x => new UserDTO { Email = x.Email, UserId = x.Id }).ToListAsync();
        }

        //[HttpGet("roles")]
        //public async Task<ActionResult<List<RolDTO>>> Get()
        //{
        //    var roles = await context.Roles
        //        .Select(x => new RolDTO { RolName = x.Name, RolId = x.Id })
        //        .ToListAsync();
        //    return Ok(roles);
        //}

        [HttpPost("asignar")]
        public async Task<ActionResult> Put(EditarRolDTO editarRolDTO)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == editarRolDTO.UserId);
            await userManager.AddToRoleAsync(usuario, editarRolDTO.RolId);
            return Ok(usuario);
        }

        [HttpPost("remover")]
        public async Task<ActionResult> Delete(EditarRolDTO editarRolDTO)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Id == editarRolDTO.UserId);
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RolId);
            return Ok(usuario);
        }
    }
}
