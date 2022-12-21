using BlazorAppPeliculasNET5.Shared.Entidades;
using System;
using System.Collections.Generic;

namespace BlazorAppPeliculasNET5.Client.Repositorios
{

    //centraliza el servicio
    public class Repositorio: IRepositorio
    {

        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula{Titulo = "Titanes del pacifico",FechaLanzamiento = new DateTime(2022,12,12)},
                new Pelicula{Titulo = "Titanes del pacifico",FechaLanzamiento = new DateTime(2015,11,11)},
                new Pelicula{Titulo = "Vengadores",FechaLanzamiento = new DateTime(2013,11,11)},
                new Pelicula{Titulo = "Gigantes de acero",FechaLanzamiento = new DateTime(2012,11,11)}
            };
        }

    }
}
