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
            var resp = await _http.PostAsJsonAsync("/Denuncia", model);
            resp.EnsureSuccessStatusCode();
            var created = await resp.Content.ReadFromJsonAsync<DenunciaViewModel>();
            return created!;
        }

        public async Task<IEnumerable<DenunciaViewModel>> GetAllByUserAsync(int userId)
        {
            var resp = await _http.GetAsync($"/Denuncia?userId={userId}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<IEnumerable<DenunciaViewModel>>() ?? Enumerable.Empty<DenunciaViewModel>();
        }

        public async Task<DenunciaViewModel> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"/Denuncia/{id}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<DenunciaViewModel>() ?? throw new Exception("Nenhum conteúdo");
        }
    }
}
