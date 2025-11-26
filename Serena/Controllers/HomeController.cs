// Local: Controllers/HomeController.cs
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serena.Models; // Mantenha apenas para o ErrorViewModel

namespace Serena.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // IMapper removido do construtor, resolvendo o erro de dependência.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // 1. Ação Principal
        public IActionResult Index()
        {
            return View();
        }

        // 2. Ação para retornar a Partial View do Login
        [HttpGet]
        public PartialViewResult LoginPartial()
        {
            // Retorna a Partial View SEM PASSAR MODEL
            return PartialView("_LoginPartial");
        }

        // 3. Ação para retornar a Partial View do Cadastro
        [HttpGet]
        public PartialViewResult CadastroPartial()
        {
            // Retorna a Partial View SEM PASSAR MODEL
            return PartialView("_CadastroPartial");
        }

        // 4. POST de Login (Apenas simulação)
        [HttpPost]
        public IActionResult Login()
        {
            // Para simulação de sucesso no front-end:
            return RedirectToAction("Dashboard");
        }

        // 5. POST do Cadastro (Apenas simulação)
        [HttpPost]
        public IActionResult Cadastrar()
        {
            // Para simulação de sucesso no front-end:
            return RedirectToAction("Dashboard");
        }

        // 6. Ação de simulação de sucesso
        public IActionResult Dashboard()
        {
            ViewData["Message"] = "Layout e Validação JavaScript funcionando! API Pendente.";
            return View();
        }

        // ... (Privacy e Error permanecem inalterados)
    }
}
