using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Serena.Models;
using Serena.Models.DTOs;

namespace Serena.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger<UserApiClient> _logger;

        // TTL: 10 minutes as requested
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

        public UserApiClient(IHttpClientFactory factory, IMemoryCache cache, IMapper mapper, ILogger<UserApiClient> logger)
        {
            _http = factory.CreateClient("ApiGateway");
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate: posts login data to downstream and returns a UserViewModel (including Password from the source model).
        /// Caches the returned UserViewModel for 10 minutes under key user_{id}.
        /// </summary>
        public async Task<UserViewModel?> AuthenticateAsync(UserViewModel dto)
        {
            if (dto == null) return null;

            try
            {

                // 1. Mapeamento
                var loginDto = _mapper.Map<UserDto>(dto);

                // 2. Chamada à API
                var resp = await _http.PostAsJsonAsync("/User/login", loginDto);

                // LOG PARA DEBUG
                var content = await resp.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {resp.StatusCode} | Body: {content}");

                // 3. CORREÇÃO DA LÓGICA: Se não for sucesso (200-299), pare aqui.
                if (!resp.IsSuccessStatusCode)
                {
                    return null; // Retorna null para o Controller tratar como "Login Inválido"
                }

                // 4. Se chegou aqui, o status é 200 OK
                var returnedDto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (returnedDto == null) return null;

                var vm = _mapper.Map<UserViewModel>(returnedDto);
                vm.Password = dto.Password;

                if (vm.Id > 0)
                {
                    var cacheKey = GetCacheKey(vm.Id);
                    _cache.Set(cacheKey, vm, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheTtl });
                }

                return vm;
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "HTTP error during AuthenticateAsync");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error during AuthenticateAsync");
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(UserViewModel dto)
        {
            if (dto == null) return false;

            try
            {
                var resp = await _http.PostAsJsonAsync("/User/reset-password", dto);
                return resp.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in ResetPasswordAsync");
                return false;
            }
        }

        public async Task<UserViewModel?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;

            var cacheKey = GetCacheKey(id);
            if (_cache.TryGetValue(cacheKey, out UserViewModel cachedVm))
            {
                return cachedVm;
            }

            try
            {
                var resp = await _http.GetAsync($"/User/{id}");
                if (resp.StatusCode == HttpStatusCode.NotFound) return null;

                resp.EnsureSuccessStatusCode();

                var dto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (dto == null) return null;

                var vm = _mapper.Map<UserViewModel>(dto);
                // do NOT set Password here (we don't know it) - keep null unless you explicitly want otherwise

                _cache.Set(cacheKey, vm, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheTtl });

                return vm;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in GetByIdAsync");
                throw;
            }
        }

        public async Task<UserViewModel?> CreateAsync(UserViewModel dto)
        {
            if (dto == null) return null;

            try
            {
                // The API probably expects a DTO; map accordingly
                var payload = _mapper.Map<UserDto>(dto);

                var resp = await _http.PostAsJsonAsync("/User", payload);

                if (resp.StatusCode == HttpStatusCode.Conflict) return null;

                resp.EnsureSuccessStatusCode();

                var createdDto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (createdDto == null) return null;

                var vm = _mapper.Map<UserViewModel>(createdDto);

                // preserve password from submitted dto (per your request)
                vm.Password = dto.Password;

                // cache the created user for 10 minutes
                if (vm.Id > 0)
                {
                    var cacheKey = GetCacheKey(vm.Id);
                    _cache.Set(cacheKey, vm, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheTtl });
                }

                return vm;
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "HTTP error in CreateAsync");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error in CreateAsync");
                throw;
            }
        }

        public async Task<UserViewModel?> UpdateAsync(int id, UserViewModel dto)
        {
            if (id <= 0 || dto == null) return null;

            try
            {
                var payload = _mapper.Map<UserDto>(dto);
                var resp = await _http.PutAsJsonAsync($"/User/{id}", payload);

                if (resp.StatusCode == HttpStatusCode.NotFound) return null;

                resp.EnsureSuccessStatusCode();

                var updatedDto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (updatedDto == null) return null;

                var vm = _mapper.Map<UserViewModel>(updatedDto);

                // preserve password if provided
                vm.Password = dto.Password;

                // refresh cache
                var cacheKey = GetCacheKey(id);
                _cache.Set(cacheKey, vm, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheTtl });

                return vm;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in UpdateAsync");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) return false;

            try
            {
                var resp = await _http.DeleteAsync($"/User/{id}");
                if (resp.StatusCode == HttpStatusCode.NotFound) return false;

                resp.EnsureSuccessStatusCode();

                // remove from cache if existed
                _cache.Remove(GetCacheKey(id));
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in DeleteAsync");
                throw;
            }
        }

        private static string GetCacheKey(int id) => $"user_{id}";
    }
}


