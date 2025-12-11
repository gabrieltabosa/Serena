using Microsoft.Extensions.Caching.Memory;
using Serena.Models.DTOs;
using System.Net;
using Serena.Models;

namespace Serena.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

        public UserApiClient(IHttpClientFactory factory, IMemoryCache cache)
        {
            _http = factory.CreateClient("ApiGateway"); // nome do client configurado
            _cache = cache;
        }

        public async Task<UserDto?> AuthenticateAsync(UserViewModel dto, CancellationToken ct = default)
        {
            var resp = await _http.PostAsJsonAsync("/User/api/User/login", dto, ct);
            if (resp.StatusCode == HttpStatusCode.Unauthorized) return null;
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<UserDto>(cancellationToken: ct);
        }

        public async Task<bool> ResetPasswordAsync(UserViewModel dto, CancellationToken ct = default)
        {
            var resp = await _http.PostAsJsonAsync("/User/api/User/reset-password", dto, ct);
            return resp.IsSuccessStatusCode;
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var cacheKey = $"user_{id}";
            if (_cache.TryGetValue(cacheKey, out UserDto cached)) return cached;

            var resp = await _http.GetAsync($"/User/api/User/{id}", ct);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;
            resp.EnsureSuccessStatusCode();
            var user = await resp.Content.ReadFromJsonAsync<UserDto>(cancellationToken: ct);
            if (user != null) _cache.Set(cacheKey, user, CacheTtl);
            return user;
        }

        public async Task<UserDto?> CreateAsync(UserViewModel dto, CancellationToken ct = default)
        {
            var resp = await _http.PostAsJsonAsync("/User/api/User", dto, ct);
            if (resp.StatusCode == HttpStatusCode.Conflict) return null;
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<UserDto>(cancellationToken: ct);
        }

        public async Task<UserDto?> UpdateAsync(int id, UserViewModel dto, CancellationToken ct = default)
        {
            var resp = await _http.PutAsJsonAsync($"/User/api/User/{id}", dto, ct);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<UserDto>(cancellationToken: ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var resp = await _http.DeleteAsync($"/User/api/User/{id}", ct);
            if (resp.StatusCode == HttpStatusCode.NotFound) return false;
            resp.EnsureSuccessStatusCode();
            return true;
        }
    }
}
