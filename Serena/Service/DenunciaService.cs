using AutoMapper;
using Serena.Models;
using Serena.Models.DTOs;


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
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new ApplicationException($"Erro ao criar denúncia: {error}");
            }
            var created = await resp.Content.ReadFromJsonAsync<DenunciaViewModel>();
            return created!;
        }

        public async Task<IEnumerable<DenunciaViewModel>> GetAllByUserAsync(int userId)
        {
            // ... código anterior ...
            var resp = await _http.GetAsync($"/Denuncias/all/{userId}");

            // EM VEZ DISSO: resp.EnsureSuccessStatusCode();
            // USE ISTO PARA DEBUGAR:
            if (!resp.IsSuccessStatusCode)
            {
                var erroDetalhado = await resp.Content.ReadAsStringAsync();

                // Isso vai aparecer no seu terminal/Output do Visual Studio
                Console.WriteLine($"-----------------------------------------");
                Console.WriteLine($"ERRO 400 DETECTADO NA ROTA: {resp.RequestMessage.RequestUri}");
                Console.WriteLine($"RESPOSTA DA API: {erroDetalhado}");
                Console.WriteLine($"-----------------------------------------");

                return Enumerable.Empty<DenunciaViewModel>(); // Retorna lista vazia em vez de quebrar
            }

            return await resp.Content.ReadFromJsonAsync<IEnumerable<DenunciaViewModel>>()
                   ?? Enumerable.Empty<DenunciaViewModel>();
        }

        public async Task<DenunciaViewModel> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"/Denuncias/{id}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<DenunciaViewModel>() ?? throw new Exception("Nenhum conteúdo");
        }
    }
}
