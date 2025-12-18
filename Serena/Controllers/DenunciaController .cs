using Microsoft.AspNetCore.Mvc;
using Serena.Models;
using Serena.Models.DTOs;
using Serena.Service;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace Serena.Controllers
{
    [Route("[controller]")]
    public class DenunciaController : Controller
    {
        private readonly IDenunciaService _denunciaService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public DenunciaController(
            IDenunciaService denunciaService,
            IMapper mapper,
            IMemoryCache cache)
        {
            _denunciaService = denunciaService;
            _mapper = mapper;
            _cache = cache;
        }

        private bool SessaoValida(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                Console.WriteLine($"SessionId inválido: '{sessionId}'");
                return false;
            }
            return _cache.TryGetValue(sessionId, out _);
        }

        private IActionResult SessaoExpirada()
        {
            return RedirectToAction("Index", "User");
        }

        

        
        [HttpGet]
        public async Task<IActionResult> Index(string sessionId, int userId)
        {
            if (!SessaoValida(sessionId))
                return SessaoExpirada();

            

            var denuncias = await _denunciaService.GetAllByUserAsync(userId);


            var model = new DashboardViewModel<DenunciaViewModel>
            {
                ActiveView = DashboardViewType.Consulta,
                Title = "Minhas Denúncias",
                Items = denuncias,
                SessionId = sessionId,
                UserId = userId
            };
           
            return View(model);
        }

        
        [HttpGet("Nova")]
        public IActionResult Nova(string sessionId, int userId)
        {
            if (!SessaoValida(sessionId))
                return SessaoExpirada();

            

            var model = new DashboardViewModel<DenunciaViewModel>
            {
                ActiveView = DashboardViewType.Cadastro,
                Title = "Registrar Nova Denúncia",
                CurrentItem = new DenunciaViewModel(),
                UserId = userId,
                SessionId = sessionId

            };
            

            return View("Index", model);
        }

       
        [HttpPost("Criar")]
        public async Task<IActionResult> Criar(
            DashboardViewModel<DenunciaViewModel> model
            )
        {
            if (!SessaoValida(model.SessionId))
                return SessaoExpirada();

            if (!ModelState.IsValid)
            {
                model.ActiveView = DashboardViewType.Cadastro;
                model.Title = "Registrar Nova Denúncia";
                return View("Index", model);
            }

            model.CurrentItem.UsuarioId = model.UserId!.Value;

            var dto = _mapper.Map<DenunciaDto>(model.CurrentItem);
            await _denunciaService.CreateAsync(dto);

            return RedirectToAction(nameof(Index), new
            {
                sessionId = model.SessionId,
                userId = model.UserId
            });
        }

      
        [HttpGet("Detalhes/{id}")]
        public async Task<IActionResult> Detalhes(
            int id,
            string sessionId,
            int userId)
        {
            if (!SessaoValida(sessionId))
                return SessaoExpirada();

            

            var denuncia = await _denunciaService.GetByIdAsync(id);

            if (denuncia == null)
                return NotFound();

            var model = new DashboardViewModel<DenunciaViewModel>
            {
                ActiveView = DashboardViewType.Atualizacao,
                Title = "Detalhes da Denúncia",
                CurrentItem = denuncia,
                SessionId = sessionId,
                UserId = userId
            };
         

            return View("Index", model);
        }
    }
}

