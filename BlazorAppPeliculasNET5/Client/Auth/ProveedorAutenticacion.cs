using BlazorAppPeliculasNET5.Client.Helpers;
using BlazorAppPeliculasNET5.Client.Repositorios;
using BlazorAppPeliculasNET5.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime js;
        private readonly HttpClient http;
        private readonly IRepositorio repositorio;
        private static readonly string TOKENKEY = "TOKENKEY";
        private static readonly string EXPIRATIONTOKENKEY = "EXPIRATIONTOKENKEY";

        private AuthenticationState anonimo =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public ProveedorAutenticacion(IJSRuntime js, HttpClient http, IRepositorio repositorio)
        {
            this.js = js;
            this.http = http;
            this.repositorio = repositorio;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.GetItemLocalStorage(TOKENKEY);
            if (string.IsNullOrEmpty(token))
            {
                return anonimo;
            }

            var timeExpirationObject = await js.GetItemLocalStorage(EXPIRATIONTOKENKEY);
            DateTime timeExpiration;
            
            if (timeExpirationObject == null)
            {
                await Limpiar();
                return anonimo;
            }

            if (DateTime.TryParse(timeExpirationObject.ToString(),out timeExpiration))
            {
                if (TokenExpirado(timeExpiration))
                {
                    await Limpiar();
                    return anonimo;
                }
                if (DebeRenovarToken(timeExpiration))
                {
                    token = await RenovarToken(token);
                }
            }
            return ConstruirAuthenticationState(token);
        }
        public async Task ManejarRenovacion()
        {
            var timeExporationObject = await js.GetItemLocalStorage(EXPIRATIONTOKENKEY);
            DateTime timeExpiration;

            if(DateTime.TryParse(timeExporationObject,out timeExpiration))
            {
                if (TokenExpirado(timeExpiration))
                {
                    await Logout();
                }
                if (DebeRenovarToken(timeExpiration))
                {
                    var token = await js.GetItemLocalStorage(TOKENKEY);
                    var nuevoToken = await RenovarToken(token);
                    var authState = ConstruirAuthenticationState(nuevoToken);
                    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                }
            }
        }
        //verifica si el token a expirado
        private bool TokenExpirado(DateTime timeExpiration)
        {
            return timeExpiration <= DateTime.UtcNow;
        }

        //Verifica si el token debe de renovarse
        private bool DebeRenovarToken(DateTime timeExpiratin)
        {
            return timeExpiratin.Subtract(DateTime.UtcNow) < TimeSpan.FromHours(1);
        }

        private async Task<string> RenovarToken(string token)
        {
            http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", token);
            var nuevoTokenResponse = await repositorio.Get<UserToken>("api/cuentas/renovar");
            var nuevoToken = nuevoTokenResponse.Response;

            await js.SetItemLocalStorage(TOKENKEY, nuevoToken.token);
            await js.SetItemLocalStorage(EXPIRATIONTOKENKEY, nuevoToken.expiration.ToString());

            return nuevoToken.token;
        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity(ParseClaimFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jwtbytes = ParsBase64WithoutPadding(payload);
            var keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(jwtbytes);

            keyValues.TryGetValue(ClaimTypes.Role, out object role);

            if (role != null)
            {
                if (role.ToString().Trim().StartsWith("["))
                {
                    var parseRoles = JsonSerializer.Deserialize<string[]>(role.ToString());
                    foreach (var parseRole in parseRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parseRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                keyValues.Remove(ClaimTypes.Role);
            }
            claims.AddRange(keyValues.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParsBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(UserToken tokenDTO)
        {
            await js.SetItemLocalStorage(TOKENKEY, tokenDTO.token);
            await js.SetItemLocalStorage(EXPIRATIONTOKENKEY, tokenDTO.expiration.ToString());
            var authState = ConstruirAuthenticationState(tokenDTO.token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await Limpiar();
            NotifyAuthenticationStateChanged(Task.FromResult(anonimo));
        }

        private async Task Limpiar()
        {
            await js.RemoveItemLocalStorage(TOKENKEY);
            await js.RemoveItemLocalStorage(EXPIRATIONTOKENKEY);
            http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
