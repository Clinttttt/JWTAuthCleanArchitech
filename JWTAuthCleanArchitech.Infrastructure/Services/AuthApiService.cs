using Azure;
using JWTAuthCleanArchitech.Domain.DTOs;
using JWTAuthCleanArchitech.Domain.Entities;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace JWTAuthCleanArchitech.WebUI.Server.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _http;
        private readonly ProtectedLocalStorage _localStorage;
        public AuthApiService(HttpClient http, ProtectedLocalStorage localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }
        public async Task<TokenResponseDto?> LoginAsync(UserDto user)

        {
            var response = await _http.PostAsJsonAsync("api/Auth/Login", user);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        }
        public async Task<User?> RegisterAsync(UserDto user)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/Register", user);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<User>();

        }
        public async Task<bool> LogoutAsync(string Accesstoken)
        {
          _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Accesstoken);
            var response = await _http.PostAsync("api/Auth/LogoutUser",null);

            _http.DefaultRequestHeaders.Authorization = null;
            return response.IsSuccessStatusCode;
        }
        public async Task<TokenResponseDto?> BecomeAdminAsync()
        {
            // 1. Get token from local storage
            var tokenResult = await _localStorage.GetAsync<string>("access_token");
            var token = tokenResult.Success ? tokenResult.Value : null;

            if (string.IsNullOrWhiteSpace(token))
                return null;

            // 2. Prepare request with Authorization header
            var request = new HttpRequestMessage(HttpMethod.Post, "api/Auth/BecomeAdmin");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3. Send request
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // 4. Parse response
            return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        }


    }
}
