using System.Net.Http.Json;
using PerezEvaluacionProgreso.Models;

namespace PerezEvaluacionProgreso.Services
{

    public class PeliculaJPService
    {
        private readonly HttpClient _httpClient;

        public PeliculaJPService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<PeliculaJP?> BuscarPeliculaAsync(string query)
        {
            try
            {
                var url = $"https://freetestapi.com/api/v1/movies?search={query}&limit=1";
                var peliculas = await _httpClient.GetFromJsonAsync<List<dynamic>>(url);

                if (peliculas == null || !peliculas.Any())
                {
                    return null;
                }

                var peliculaApi = peliculas.FirstOrDefault();

                return new PeliculaJP
                {
                    Nombre = peliculaApi.title,
                    Genero = string.Join(", ", peliculaApi.genre),
                    Director = peliculaApi.director,
                    Year = peliculaApi.year.ToString(),
                    PosterUrl = peliculaApi.poster,
                    Sinopsis = peliculaApi.plot,
                    Actores = string.Join(", ", peliculaApi.actors),
                    Rating = peliculaApi.rating
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo encontrar la API *Error al llamarla*: {ex.Message}");
                return null;
            }
        }
    }
}