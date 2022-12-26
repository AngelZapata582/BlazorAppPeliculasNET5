using BlazorAppPeliculasNET5.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Shared.DTOs
{
	public class VisualizarPeliculaDTO
	{
		public Pelicula Pelicula { get; set; }
		public List<Genero> Generos { get; set; }
		public List<Actor> Actores { get; set; }
		public int Voto { get; set; }
		public double Promedio { get; set; }
	}
}
