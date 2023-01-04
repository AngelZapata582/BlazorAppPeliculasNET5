using BlazorAppPeliculasNET5.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Controllers
{
    [Route("api/[controller]")]
    public class CuentasController:ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager; //logear usuarios
        private readonly SignInManager<IdentityUser> _signInManager; //registrar usuarios
        private readonly IConfiguration _configuration; //acceder a las configuraciones para la llave de jwt

        public CuentasController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            
            var result = await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Correo o contraseña invalidas");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password,
                isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await BuildToken(user);
            }
            else
            {
                return BadRequest("Credenciales no validas");
            }
        }

        [HttpGet("renovar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Get()
        {
            var userInfo = new UserInfo()
            {
                Email = HttpContext.User.Identity.Name
            };
            return await BuildToken(userInfo);
        }

        //metodo independiente del framework blazor y sql, crea un token jwt a partir de cualquier info
        private async Task<UserToken> BuildToken(UserInfo model)
        {
            var clains = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, model.Email),
                new Claim(ClaimTypes.Name, model.Email),
                new Claim("miValor","Cualquier valor que sea"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var usuario = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(usuario);
            foreach (var rol in roles)
            {
                clains.Add(new Claim(ClaimTypes.Role,rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(9);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience:null,
                claims:clains,
                expires:expiration,
                signingCredentials:creds);

            return new UserToken
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration,
            };
        }
    }
}
