using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Repositorios
{
    public class HttpResponceWrapper<T>
    {
        public HttpResponceWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> ObtenerMensageError()
        {
            if (!Error)
            {
                return null;
            }
            var codigoEstatus = HttpResponseMessage.StatusCode;
            if(codigoEstatus == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }else if (codigoEstatus == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }else if(codigoEstatus == HttpStatusCode.Unauthorized)
            {
                return "Tienes que iniciar sesion";
            }else if(codigoEstatus == HttpStatusCode.Forbidden)
            {
                return "No tienes autorizado hacer esta accion";
            }
            else
            {
                return "Ha ocurrido un error inesperado";
            }
        }
    }
}
