using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serena.Models;         // UserViewModel
using Serena.Service;        // IUserApiClient
using System;
using System.Threading.Tasks;

namespace Serena.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserApiClient userApiClient, ILogger<UserController> logger)
        {
            _userApiClient = userApiClient ?? throw new ArgumentNullException(nameof(userApiClient));
            _logger = logger;
        }

        // GET /User/ShowLogin -> seta TempData e redireciona para Home/Index
        [HttpGet("ShowLogin")]
        public IActionResult ShowLogin()
        {
            TempData["TelaAtual"] = "Login";
            return RedirectToAction("Index", "Home");
        }

        // GET /User/ShowCadastro -> seta TempData e redireciona para Home/Index
        [HttpGet("ShowCadastro")]
        public IActionResult ShowCadastro()
        {
            TempData["TelaAtual"] = "Cadastro";
            return RedirectToAction("Index", "Home");
        }

        // POST /User/Login -> form submit server-side
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] UserViewModel model)
        {
            // validação server-side básica
            if (!ModelState.IsValid)
            {
                TempData["TelaAtual"] = "Login";
                ViewData["UserModel"] = model; // para repassar o model ao Home/Index e partial
                return View("~/Views/Home/Index.cshtml");
            }

            try
            {
                var userDto = await _userApiClient.AuthenticateAsync(model);

                if (userDto == null)
                {
                    // credenciais inválidas -> mantém tela de login com erro
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas.");
                    TempData["TelaAtual"] = "Login";
                    ViewData["UserModel"] = model;
                    return View("~/Views/Home/Index.cshtml");
                }

                // login bem sucedido -> redireciona para Home/Index mostrando outra tela
                TempData["TelaAtual"] = "ConsultaDenuncias";
                TempData["WelcomeName"] = userDto.Name;
                // opcional: armazenar informações na sessão, se precisar
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao autenticar usuário.");
                ModelState.AddModelError(string.Empty, "Erro ao conectar ao serviço de autenticação. Tente novamente.");
                TempData["TelaAtual"] = "Login";
                ViewData["UserModel"] = model;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        // POST /User/Create -> form submit server-side
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["TelaAtual"] = "Cadastro";
                ViewData["UserModel"] = model;
                return View("~/Views/Home/Index.cshtml");
            }

            try
            {
                var created = await _userApiClient.CreateAsync(model);

                if (created == null)
                {
                    // conflito ou regra de negócio
                    ModelState.AddModelError(string.Empty, "Não foi possível criar o usuário (usuário já existe ou conflito).");
                    TempData["TelaAtual"] = "Cadastro";
                    ViewData["UserModel"] = model;
                    return View("~/Views/Home/Index.cshtml");
                }

                // criado com sucesso: redireciona para Home/Index e mostra o Login (para o usuário entrar)
                TempData["TelaAtual"] = "Login";
                TempData["CreatedUserName"] = created.Name;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao criar usuário.");
                ModelState.AddModelError(string.Empty, "Erro interno ao criar usuário. Tente novamente.");
                TempData["TelaAtual"] = "Cadastro";
                ViewData["UserModel"] = model;
                return View("~/Views/Home/Index.cshtml");
            }
        }
    }
}
