using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serena.Models;
using Serena.Service;
using System.Security.Claims;

namespace Serena.Controllers
{
    [Route("[controller]")]
    [Authorize] // Protege o perfil por padrão
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        // --- 1. Tela de Login (Ponto de Entrada) ---
        [HttpGet]
        [AllowAnonymous]
        [Route("")]      // Atende em: /User
        [Route("~/")]    // Atende em: / (Raiz do site)
        [Route("Index")]
        public IActionResult Index()
        {
            var model = new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Login,
                Title = "Acesso ao Sistema",
                CurrentItem = new UserViewModel()
            };
            return View(model);
        }

        // --- 2. Fluxo de Cadastro ---
        [HttpGet("ViewCadastro")]
        [AllowAnonymous]
        public IActionResult ViewCadastro()
        {
            var viewModel = new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Cadastro,
                Title = "Crie sua Conta",
                CurrentItem = new UserViewModel()
            };
            return View("Index", viewModel);
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new DashboardViewModel<UserViewModel>
                {
                    ActiveView = DashboardViewType.Cadastro,
                    CurrentItem = model
                });
            }

            var newUser = await _userApiClient.CreateAsync(model);
            if (newUser == null)
            {
                ModelState.AddModelError(string.Empty, "Erro ao cadastrar. Tente outro e-mail.");
                return View("Index", new DashboardViewModel<UserViewModel>
                {
                    ActiveView = DashboardViewType.Cadastro,
                    CurrentItem = model
                });
            }

            return RedirectToAction(nameof(Index)); // Volta para o login após cadastrar
        }

        // --- 3. Fluxo de Atualização ---
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userApiClient.GetByIdAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            var viewModel = new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Atualizacao,
                Title = "Meu Perfil",
                CurrentItem = user
            };
            return View("Index", viewModel);
        }

        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new DashboardViewModel<UserViewModel>
                {
                    ActiveView = DashboardViewType.Atualizacao,
                    CurrentItem = model
                });
            }

            await _userApiClient.UpdateAsync(model.Id, model);
            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }

        // --- 4. Fluxo de Exclusão ---
        [HttpGet("ConfirmDelete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var user = await _userApiClient.GetByIdAsync(id);
            var viewModel = new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Exclusao,
                Title = "Excluir Conta",
                CurrentItem = user
            };
            return View("Index", viewModel);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userApiClient.DeleteAsync(id);

            // AJUSTE: Se o usuário logado está deletando a própria conta, 
            // precisamos limpar o cookie para que ele não tente acessar áreas restritas
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == id.ToString())
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            // 1. Validar via API (Supondo que sua API tenha um método de autenticação)
            // Se a sua API retorna o usuário completo ao validar a senha:
            var user = await _userApiClient.AuthenticateAsync(model);
            

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View("Index", new DashboardViewModel<UserViewModel>
                {
                    ActiveView = DashboardViewType.Login,
                    CurrentItem = model
                });
            }

            // 2. Criar as credenciais (Claims) para o sistema de Cookies do ASP.NET Core
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // 3. Configurar a expiração de 10 minutos aqui também (Garantia extra)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false, // Mantém o cookie mesmo se fechar o navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // 4. Redirecionar para o perfil (Edição) ou página de denúncias
            return RedirectToAction("Index", "Denuncia");
        }

        // --- 6. Ação de Logout ---
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }
}
