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
                new Pelicula{Titulo = "Titanes del pacifico",
                    Lanzamiento = new DateTime(2013,12,12),
                    Poster = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQwDSCJ8nC3Lsy__8NKMKLlRFmKqsljYzpBHOtnvMckFn4IEZTV"},
                new Pelicula{Titulo = "Your name",
                    Lanzamiento = new DateTime(2016,11,11),
                    Poster = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcTAHTm5DX_IpGYYrE08mnYB8-6kVd17RUwJvgr7x8IwGcNqi1K7"},
                new Pelicula{Titulo = "Vengadores",
                    Lanzamiento = new DateTime(2013,11,11),
                    Poster = "https://lumiere-a.akamaihd.net/v1/images/eu_disneyplus_avengers-endgame_mob_m_928f44f1.jpeg?region=100,0,600,600"},
                new Pelicula{Titulo = "Gigantes de acero",
                    Lanzamiento = new DateTime(2012,11,11),
                    Poster = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQ8o62EalzX4aotuH8dGX9m-kuX-oWZ9BKXc8IiBu18dnBW6w-6"}
            };
        }

    }
}
