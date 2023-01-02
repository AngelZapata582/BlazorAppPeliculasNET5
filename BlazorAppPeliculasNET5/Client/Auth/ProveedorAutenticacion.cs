using BlazorAppPeliculasNET5.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
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
        private static readonly string TOKENKEY = "TOKENKEY";
        private AuthenticationState anonimo =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public ProveedorAutenticacion(IJSRuntime js, HttpClient http)
        {
            this.js = js;
            this.http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.GetItemLocalStorage(TOKENKEY);
            if(string.IsNullOrEmpty(token))
            {
                return anonimo;
            }
            return ConstruirAuthenticationState(token);
        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity(ParseClaimFromJwt(token),"jwt")));
        }

        private IEnumerable<Claim> ParseClaimFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jwtbytes = ParsBase64WithoutPadding(payload);
            var keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(jwtbytes);

            keyValues.TryGetValue(ClaimTypes.Role, out object role);

            if(role != null)
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
                case 2: base64 += "==";break;
                case 3: base64 += "=";break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(string token)
        {
            await js.SetItemLocalStorage(TOKENKEY,token);
            var authState = ConstruirAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await js.RemoveItemLocalStorage(TOKENKEY);
            http.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(anonimo));
        }
    }
}
