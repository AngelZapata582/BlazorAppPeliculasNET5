using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.Entidades
{
    public class Pelicula
    {
        public string Titulo { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Poster { get; set; }
        public string TituloCortado
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Titulo))
                {
                    return null;
                }
                if(Titulo.Length > 60)
                {
                    return Titulo.Substring(0, 60) + "...";
                }
                else
                {
                    return Titulo;
                }
            }
        }
    }
}
