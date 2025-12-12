using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serena.Models;
using Serena.Models.DTOs;
using Serena.Service;
using System.Security.Claims;
using AutoMapper;

namespace Serena.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class DenunciaController : Controller
    {
        private readonly IDenunciaService _denunciaService;
        private readonly IMapper _mapper;

        public DenunciaController(IDenunciaService denunciaService, IMapper mapper)
        {
            _denunciaService = denunciaService;
            _mapper = mapper;
        }

        // Recupera o ID do usuário logado nos Cookies/Claims
        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // --- 1. Lista de Denúncias do Usuário ---
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var denuncias = await _denunciaService.GetAllByUserAsync(GetUserId());

            var model = new DashboardViewModel<DenunciaViewModel>
            {
                ActiveView = DashboardViewType.Consulta,
                Title = "Minhas Denúncias",
                Items = denuncias
            };
            return View(model);
        }

        // --- 2. Tela de Nova Denúncia ---
        [HttpGet("Nova")]
        public IActionResult Nova()
        {
            var model = new DashboardViewModel<DenunciaViewModel>
            {
                ActiveView = DashboardViewType.Cadastro,
                Title = "Registrar Nova Denúncia",
                CurrentItem = new DenunciaViewModel()
            };
            return View("Index", model);
        }

        // --- 3. POST: Criar Denúncia ---
        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(DenunciaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new DashboardViewModel<DenunciaViewModel>
                {
                    ActiveView = DashboardViewType.Cadastro,
                    CurrentItem = model
                });
            }
            var dto = _mapper.Map<DenunciaDto>(model);



            await _denunciaService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // --- 4. Ver Detalhes ---
        [HttpGet("Detalhes/{id}")]
        public async Task<IActionResult> Detalhes(int id)
        {
            var denuncia = await _denunciaService.GetByIdAsync(id);
            var model = new DashboardViewModel<DenunciaViewModel>
            {
                ActiveView = DashboardViewType.Atualizacao, // Usando como "Visualização"
                Title = "Detalhes da Denúncia",
                CurrentItem = denuncia
            };
            return View("Index", model);
        }
    }
}
