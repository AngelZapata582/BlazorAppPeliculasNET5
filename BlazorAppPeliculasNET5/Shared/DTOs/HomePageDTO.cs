using BlazorAppPeliculasNET5.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.DTOs
{
    public class HomePageDTO
    {
        public List<Pelicula> PeliculasEnCartelera { get; set; } = null!;
        public List<Pelicula> PeliculasDeEstreno { get; set; } = null!;
    }
}
