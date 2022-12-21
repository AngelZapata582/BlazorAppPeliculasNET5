using System.Collections.Generic;
using BlazorAppPeliculasNET5.Shared.Entidades;
namespace BlazorAppPeliculasNET5.Client.Repositorios
{
    //interfaz es una serie de reglas que una clase debe de segir
    //cualquier clase que implemente esta interfaz debera implementar el metodo
    public interface IRepositorio
    {
        List<Pelicula> ObtenerPeliculas();
    }
}
