using PerezEvaluacionProgreso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
                var peliculas = await _httpClient.GetFromJsonAsync<List<PeliculaJP>>(url);

                return peliculas?.FirstOrDefault(); 
            }
            catch
            {
                return null; 
            }
        }
    }
}
