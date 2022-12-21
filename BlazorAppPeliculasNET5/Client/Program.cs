using BlazorAppPeliculasNET5.Client.Repositorios;
using CurrieTechnologies.Razor.SweetAlert2;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //configura el servicio 
            //                            interfaz a    clase a
            //                            implementar   proveer
            builder.Services.AddSingleton<IRepositorio, Repositorio>();
            builder.Services.AddSweetAlert2();
            await builder.Build().RunAsync();
        }
    }
}
