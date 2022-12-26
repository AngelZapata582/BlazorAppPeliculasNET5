using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorAppPeliculasNET5.Shared.Entidades;
namespace BlazorAppPeliculasNET5.Client.Repositorios
{
    //interfaz es una serie de reglas que una clase debe de segir
    //cualquier clase que implemente esta interfaz debera implementar el metodo
    public interface IRepositorio
    {
        Task<HttpResponceWrapper<T>> Get<T>(string url);
        List<Pelicula> ObtenerPeliculas();
        Task<HttpResponceWrapper<object>> Post<T>(string url, T enviar);
        Task<HttpResponceWrapper<TResponse>> Post<T, TResponse>(string url, T enviar);
        Task<HttpResponceWrapper<object>> Put<T>(string url, T enviar);
    }
}
