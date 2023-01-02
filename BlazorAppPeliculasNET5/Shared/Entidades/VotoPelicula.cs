﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.Entidades
{
    public class VotoPelicula
    {
        public int Id { get; set; }
        public int Voto { get; set; }
        public DateTime FechaVoto { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set;}
        public string UserId { get; set; }
    }
}
