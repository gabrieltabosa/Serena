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

        // Tempo máximo que os dados podem ficar em cache (10 minutos)
        // Isso NÃO é controle de autenticação, apenas otimização
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

        public UserApiClient(
            IHttpClientFactory factory,
            IMemoryCache cache,
            IMapper mapper,
            ILogger<UserApiClient> logger)
        {
            _http = factory.CreateClient("ApiGateway");
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<UserViewModel?> AuthenticateAsync(UserViewModel dto)
        {
            if (dto == null) return null;

            try
            {
                var loginDto = _mapper.Map<UserDto>(dto);
                var resp = await _http.PostAsJsonAsync("/User/login", loginDto);

                if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    return null;

                if (!resp.IsSuccessStatusCode)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogWarning("Falha login: {StatusCode} - {Error}", resp.StatusCode, error);
                    return null;
                }

                var returnedDto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (returnedDto == null) return null;

                var vm = _mapper.Map<UserViewModel>(returnedDto);
                vm.Password = null;
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em AuthenticateAsync");
                return null;
            }
        }



        public async Task<UserViewModel?> GetByIdAsync(int id, string sessionId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(sessionId))
                return null;

            var cacheKey = GetCacheKey(id, sessionId);

            if (_cache.TryGetValue(cacheKey, out UserViewModel cached))
                return cached;

            try
            {
                var resp = await _http.GetAsync($"/User/{id}");

                if (resp.StatusCode == HttpStatusCode.NotFound)
                    return null;

                if (!resp.IsSuccessStatusCode)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogWarning("Erro GetById: {StatusCode} - {Error}", resp.StatusCode, error);
                    return null;
                }

                var dto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (dto == null) return null;

                var vm = _mapper.Map<UserViewModel>(dto);
                vm.Password = null;

                _cache.Set(cacheKey, vm, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheTtl
                });

                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em GetByIdAsync");
                return null;
            }
        }


        public async Task<UserViewModel?> CreateAsync(UserViewModel dto)
        {
            if (dto == null) return null;

            try
            {
                var payload = _mapper.Map<UserDto>(dto);
                var resp = await _http.PostAsJsonAsync("/User", payload);

                if (resp.StatusCode == HttpStatusCode.Conflict ||
                    resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogWarning("Falha CreateAsync: {StatusCode} - {Error}", resp.StatusCode, error);
                    return null;
                }

                if (!resp.IsSuccessStatusCode)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogError("Erro inesperado CreateAsync: {StatusCode} - {Error}", resp.StatusCode, error);
                    return null;
                }

                var createdDto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (createdDto == null) return null;

                var vm = _mapper.Map<UserViewModel>(createdDto);
                vm.Password = null;
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em CreateAsync");
                return null;
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
                _logger?.LogError(ex, "Error in ResetPasswordAsync"); return false; 
            } 
        }
        public async Task<UserViewModel?> UpdateAsync(int id, UserViewModel dto)
        {
            if (id <= 0 || dto == null) return null;

            try
            {
                var payload = _mapper.Map<UserDto>(dto);
                var resp = await _http.PutAsJsonAsync($"/User/{id}", payload);

                if (resp.StatusCode == HttpStatusCode.NotFound ||
                    resp.StatusCode == HttpStatusCode.BadRequest ||
                    resp.StatusCode == HttpStatusCode.Conflict)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogWarning("Falha UpdateAsync: {StatusCode} - {Error}", resp.StatusCode, error);
                    return null;
                }

                if (!resp.IsSuccessStatusCode)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogError("Erro inesperado UpdateAsync: {StatusCode} - {Error}", resp.StatusCode, error);
                    return null;
                }

                var updatedDto = await resp.Content.ReadFromJsonAsync<UserDto>();
                if (updatedDto == null) return null;

                var vm = _mapper.Map<UserViewModel>(updatedDto);
                vm.Password = null;
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em UpdateAsync");
                return null;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) return false;

            try
            {
                var resp = await _http.DeleteAsync($"/User/{id}");

                if (resp.StatusCode == HttpStatusCode.NotFound ||
                    resp.StatusCode == HttpStatusCode.BadRequest)
                    return false;

                if (!resp.IsSuccessStatusCode)
                {
                    var error = await resp.Content.ReadAsStringAsync();
                    _logger.LogError("Erro DeleteAsync: {StatusCode} - {Error}", resp.StatusCode, error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado em DeleteAsync");
                return false;
            }
        }

        
        private static string GetCacheKey(int userId, string sessionId)
            => $"session_{sessionId}_user_{userId}";
    }
}
