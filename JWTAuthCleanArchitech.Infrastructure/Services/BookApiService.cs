/*using Azure;
using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Models;
using System.Net.Http.Json;

namespace JWTAuthCleanArchitech.WebUI.Server.Services
{
    public class BookApiService
    {
        private readonly HttpClient _http;
        public BookApiService (HttpClient http)
        {
            _http = http;
        }
        public void SetToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<List<Movies>?> GetAllAsyc()
        {
            return await _http.GetFromJsonAsync<List<Movies>>("api/Auth/GetAllProduct");
        }
        public async Task<Movies?> GetById(Guid id)
        {
            return await _http.GetFromJsonAsync<Movies>($"api/Auth/AddBook/{id}");
        }
        public async Task<Movies?> AddBook(MoviesDto moviedto)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/AddBook", moviedto);
              return await response.Content.ReadFromJsonAsync<Movies>();
        }
        public async Task<bool> DeleteBook (Guid id)
        {
            var response = await _http.DeleteAsync($"api/Auth/DeleteMovies/{id}");
            return  response.IsSuccessStatusCode;
        }
        public async Task<Movies?> UpdateBook(Movies movie)
        {
            var response = await _http.PutAsJsonAsync($"api/Auth/UpdateMovies",movie);
            return await response.Content.ReadFromJsonAsync<Movies>();
        }
      
    }
}
*/



using Azure;
using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Json;

namespace JWTAuthCleanArchitech.WebUI.Server.Services
{
    public class BookApiService
    {
        private readonly HttpClient _http;
        private readonly ProtectedLocalStorage _storage;

        public BookApiService(HttpClient http, ProtectedLocalStorage storage)
        {
            _http = http;
            _storage = storage;
        }

        // Simple method to get token and set it in header
        private async Task SetAuthHeaderAsync()
        {
            var result = await _storage.GetAsync<string>("access_token");
            var token = result.Success ? result.Value : null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Movies>?> GetAllAsyc()
        {
            await SetAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<Movies>>("api/Auth/GetAllProduct");
        }

        public async Task<Movies?> GetById(Guid id)
        {
            await SetAuthHeaderAsync();
            return await _http.GetFromJsonAsync<Movies>($"api/Auth/AddBook/{id}");
        }

        public async Task<Movies?> AddBook(MoviesDto moviedto)
        {
            await SetAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/Auth/AddBook", moviedto);
            return await response.Content.ReadFromJsonAsync<Movies>();
        }

        public async Task<bool> DeleteBook(Guid id)
        {
            await SetAuthHeaderAsync();
            var response = await _http.DeleteAsync($"api/Auth/DeleteMovies/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Movies?> UpdateBook(Movies movie)
        {
            await SetAuthHeaderAsync();
            var response = await _http.PutAsJsonAsync($"api/Auth/UpdateMovies", movie);
            return await response.Content.ReadFromJsonAsync<Movies>();
        }
    }
}