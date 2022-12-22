using System.Net.Http;

namespace BlazorAppPeliculasNET5.Client.Repositorios
{
    public class HttpResponceWrapper<T>
    {
        public HttpResponceWrapper(T? response, bool error,HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }
}
