using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Server.Helpers
{
    public interface IAlmacenamiento
    {
        Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenido);
        Task EliminarArchivo(string ruta, string nombreContenedor);
        async Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenido, string ruta)
        {
            if(ruta is not null)
            {
                await EliminarArchivo(ruta, nombreContenido);
            }
            return await GuardarArchivo(contenido, extension, nombreContenido);
        }
    }
}
