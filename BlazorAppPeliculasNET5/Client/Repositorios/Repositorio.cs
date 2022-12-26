using BlazorAppPeliculasNET5.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorAppPeliculasNET5.Client.Repositorios
{

    //centraliza el servicio
    public class Repositorio : IRepositorio
    {
        private readonly HttpClient _httpClient;

        public Repositorio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private JsonSerializerOptions OpcionesDefaults => new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve, //evita el error ciclico de referencia de los modelos al hacer una peticion
            PropertyNameCaseInsensitive = true
        };

        public async Task<HttpResponceWrapper<T>> Get<T>(string url)
        {
            var respuestaHttp = await _httpClient.GetAsync(url);
            if (respuestaHttp.IsSuccessStatusCode)
            {
                var respuesta = await DeserializarObjeto<T>(respuestaHttp, OpcionesDefaults);
                return new HttpResponceWrapper<T>(respuesta, error: false, respuestaHttp);
            }
            else
            {
                return new HttpResponceWrapper<T>(default, error: true, respuestaHttp);
            }
        }

        public async Task<HttpResponceWrapper<object>> Post<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContext = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, enviarContext);
            return new HttpResponceWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponceWrapper<TResponse>> Post<T, TResponse>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContext = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, enviarContext);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializarObjeto<TResponse>(responseHttp, OpcionesDefaults);
                return new HttpResponceWrapper<TResponse>(response, error: false, responseHttp);
            }

            return new HttpResponceWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> DeserializarObjeto<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonOptions)
        {
            var respuestString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestString, jsonOptions);
        }

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
