using BlazorAppPeliculasNET5.Client.Auth;
using BlazorAppPeliculasNET5.Client.Repositorios;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(sp => 
            new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //configura el servicio 
            //                            interfaz a    clase a
            //                            implementar   proveer
            builder.Services.AddScoped<IRepositorio, Repositorio>();
            builder.Services.AddSweetAlert2();
            builder.Services.AddAuthorizationCore();
            //registra como servico la clase de ProveedorAutenticacion
            builder.Services.AddScoped<ProveedorAutenticacion>();
            //Crea una instancia del servicio de ProveedorAutenticacion
            builder.Services.AddScoped<AuthenticationStateProvider,ProveedorAutenticacion>(provider => 
                provider.GetRequiredService<ProveedorAutenticacion>());

            builder.Services.AddScoped<ILoginService, ProveedorAutenticacion>(provider =>
                provider.GetRequiredService<ProveedorAutenticacion>());
            await builder.Build().RunAsync();
        }
    }
}
