using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Helpers
{
    public class AlmacenamientoLocal : IAlmacenamiento
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenamientoLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(env.WebRootPath, nombreContenedor);
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenido)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var folder = Path.Combine(env.WebRootPath, nombreContenido);
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string rutaGuardado = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(rutaGuardado, contenido);

            var uriActual = $"{httpContextAccessor!.HttpContext!.Request.Scheme}//{httpContextAccessor.HttpContext.Request.Host}";
            var rutaDb = Path.Combine(uriActual, nombreContenido, nombreArchivo).Replace("\\","/");//remplaza la barra invertida por la barra hacia delante
            return rutaDb;
        }
    }
}
