using BlazorAppPeliculasNET5.Client.Repositorios;
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

            builder.Services.AddSingleton<ServicioSingleton>();
            builder.Services.AddTransient<ServicioTransient>();
            //configura el servicio 
            //                            interfaz a    clase a
            //                            implementar   proveer
            builder.Services.AddSingleton<IRepositorio, Repositorio>();

            await builder.Build().RunAsync();
        }
    }
}
