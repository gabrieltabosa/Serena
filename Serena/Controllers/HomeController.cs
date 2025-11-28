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
        public IActionResult Login(string Email, string Senha)
        {
            // 1. **SE O LOGIN FALHAR** (Exemplo)
            if (Email == "teste@teste.com")
            {
                // Define a flag e retorna a Partial View de login novamente
                ViewBag.LoginFalhou = true;
                return PartialView("_LoginPartial");
            }

            // 2. **SE O LOGIN FOR BEM-SUCEDIDO** (Fluxo correto)
            // Não retorna o _LoginPartial, retorna a próxima tela
            return PartialView("~/Views/Ativos/_ConsultaAtivos.cshtml"); // ou retorna Json({success: true}) para AJAX
        }
        

        
        [HttpPost]
        public IActionResult Cadastrar()
        {
            // *** IMPORTANTE: Simula um cadastro BEM-SUCEDIDO. ***
            // Na prática, você faria a validação aqui.

            // Retorna um JSON que o JavaScript (validador.js) interpretará como sucesso, 
            // disparando o redirecionamento para ConsultaPartial.
            return Json(new { success = true });
        }

       
        

        // ... (Privacy e Error permanecem inalterados)
    }
}
