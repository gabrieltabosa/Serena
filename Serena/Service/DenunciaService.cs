using AutoMapper;
using DominioSerena;
using DominioSerena.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Serena.Service
{
    public class DenunciaService: IDenunciaService
    {
        private readonly HttpClient _http;
        private readonly IMapper _mapper;

        public DenunciaService(IHttpClientFactory factory, IMapper mapper)
        {
            _http = factory.CreateClient("ApiGateway");
            _mapper = mapper;
        }

        public async Task<DenunciaViewModel> CreateAsync(DenunciaDto model)
        {
            var resp = await _http.PostAsJsonAsync("/Denuncias", model);
            var jsonPuro =  await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new ApplicationException($"Erro ao criar denúncia: {error}");
            }
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // Adicionamos o conversor para suportar o nome técnico do Enum (ex: "violenciaFisica")
                options.Converters.Add(new JsonStringEnumConverter());
                // Desserializamos a string que já temos em mãos
                return JsonSerializer.Deserialize<DenunciaViewModel>(jsonPuro, options)
                       ?? throw new ApplicationException("Erro ao desserializar a denúncia criada.");

            }
            catch (JsonException ex)
            {
                Console.WriteLine($"!!! ERRO DE CONVERSÃO NO JSON !!!");
                Console.WriteLine($"Mensagem: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DenunciaViewModel>> GetAllByUserAsync(int userId)
        {
            var resp = await _http.GetAsync($"/Denuncias/all/{userId}");

            // 1. Lemos o conteúdo bruto como string primeiro
            var jsonPuro = await resp.Content.ReadAsStringAsync();

            

            if (!resp.IsSuccessStatusCode)
            {
                return Enumerable.Empty<DenunciaViewModel>();
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // Adicionamos o conversor para suportar o nome técnico do Enum (ex: "violenciaFisica")
                options.Converters.Add(new JsonStringEnumConverter());

                // Desserializamos a string que já temos em mãos
                var lista = JsonSerializer.Deserialize<IEnumerable<DenunciaDto>>(jsonPuro, options)
                       ?? Enumerable.Empty<DenunciaDto>();

                foreach (var denuncia in lista)
                {
                    if (!String.IsNullOrEmpty(denuncia.Descricao))
                    {
                        denuncia.Descricao = denuncia.Descricao.Trim('"');
                    }
                }
                return _mapper.Map<IEnumerable<DenunciaViewModel>>(lista);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"!!! ERRO DE CONVERSÃO NO JSON !!!");
                Console.WriteLine($"Mensagem: {ex.Message}");
                // Aqui você vai descobrir se a API enviou "Violencia Fisica" (com espaço)
                // ou um número que o seu Enum não conhece.
                throw;
            }
        }

        public async Task<DenunciaViewModel> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"/Denuncias/{id}");
            var jsonPuro = await resp.Content.ReadAsStringAsync();
            
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // Adicionamos o conversor para suportar o nome técnico do Enum (ex: "violenciaFisica")
                options.Converters.Add(new JsonStringEnumConverter());

                // Desserializamos a string que já temos em mãos
                DenunciaDto denuncia = JsonSerializer.Deserialize<DenunciaDto>(jsonPuro, options)
                      ?? new DenunciaDto();

                
                if (denuncia.Descricao != null)
                {
                    denuncia.Descricao = denuncia.Descricao.Trim('"');
                }
                
                return _mapper.Map<DenunciaViewModel>(denuncia);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"!!! ERRO DE CONVERSÃO NO JSON !!!");
                Console.WriteLine($"Mensagem: {ex.Message}");
                // Aqui você vai descobrir se a API enviou "Violencia Fisica" (com espaço)
                // ou um número que o seu Enum não conhece.
                throw;
            }

        }
    }
}
