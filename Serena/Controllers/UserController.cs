using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serena.Models;
using Serena.Service;
using System.Reflection;

namespace Serena.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IMemoryCache _cache;
       
        

        public UserController(IUserApiClient userApiClient, IMemoryCache cache)
        {
            _userApiClient = userApiClient;
            _cache = cache;
        }
        

       
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        [Route("~/")]
        [Route("Index")]
        public IActionResult Index( )
        {
            return View(new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Login,
                Title = "Acesso ao Sistema",
                CurrentItem = new UserViewModel()
            });
        }

      
        
        [HttpGet("ViewCadastro")]
        [AllowAnonymous]
        public IActionResult ViewCadastro()
        {
            return View("Index", new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Cadastro,
                Title = "Crie sua Conta",
                CurrentItem = new UserViewModel()
            });
        }

        [HttpPost("Create")]
        [AllowAnonymous]
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

            try
            {
                var newUser = await _userApiClient.CreateAsync(model);

                if (newUser == null)
                {
                    // Fallback genérico caso não venha detalhe
                    ModelState.AddModelError(string.Empty, "Não foi possível cadastrar o usuário.");
                    return View("Index", new DashboardViewModel<UserViewModel>
                    {
                        ActiveView = DashboardViewType.Cadastro,
                        CurrentItem = model
                       
                    });
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                
                TryAddApiErrorsToModelState(ex.Message);

                return View("Index", new DashboardViewModel<UserViewModel>
                {
                    ActiveView = DashboardViewType.Cadastro,
                    CurrentItem = model
                });
            }
        }

        private void TryAddApiErrorsToModelState(string erroJson)
        {
            try
            {
                var problem = System.Text.Json.JsonSerializer.Deserialize<ApiValidationProblem>(erroJson);

                if (problem?.Errors != null)
                {
                    foreach (var field in problem.Errors)
                    {
                        foreach (var message in field.Value)
                        {
                            ModelState.AddModelError(field.Key, message);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao processar a solicitação.");
                }
            }
            catch
            {
                // Se não for JSON válido
                ModelState.AddModelError(string.Empty, erroJson);
            }
        }

        
        [HttpGet("Edit/{userId}")]
        public async Task<IActionResult> Edit(int userId, string sessionId)
        {
            if (!SessaoValida(sessionId))
                return RedirectToAction(nameof(Index));

            // 🔑 Interface corrigida: agora exige sessionId
            var user = await _userApiClient.GetByIdAsync(userId, sessionId);
            if (user == null)
                return RedirectToAction(nameof(Index));
            Console.WriteLine($"o nome do usuario atualizado é: {user.Name}");

            return View("Index", new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Atualizacao,
                Title = "Meu Perfil",
                CurrentItem = user,
                SessionId = sessionId,
                UserId = userId
            });
        }
        

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DashboardViewModel<UserViewModel> model)
        {
            if (!SessaoValida(model.SessionId))
                return RedirectToAction(nameof(Index));

            
            
            model.CurrentItem = await _userApiClient.UpdateAsync(model.CurrentItem.Id, model.CurrentItem, model.SessionId);
            
            
            model.ActiveView = DashboardViewType.Atualizacao;
            return View("Index", model);
        }

        
        [HttpGet("ConfirmDelete/{userId}")]
        public async Task<IActionResult> ConfirmDelete(int userId, string sessionId)
        {
            Console.WriteLine($"tempo restante da sessao: {sessionId}");
            if (!SessaoValida(sessionId))
                return RedirectToAction(nameof(Index));

            var user = await _userApiClient.GetByIdAsync(userId, sessionId);

            Console.WriteLine($"o nome do usuario atualizado é: {user.Name}");
            return View("Index", new DashboardViewModel<UserViewModel>
            {
                ActiveView = DashboardViewType.Exclusao,
                Title = "Excluir Conta",
                CurrentItem = user,
                SessionId = sessionId,
                UserId = userId
            });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int userId, string sessionId)
        {
            if (!SessaoValida(sessionId))
                return RedirectToAction(nameof(Index));

            await _userApiClient.DeleteAsync(userId,sessionId);

            
            

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserViewModel model)
        {
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

            // Cria sessão em memória (10 min)
            var sessionId = Guid.NewGuid().ToString();
            _cache.Set(sessionId, true, TimeSpan.FromMinutes(5));
            

            // Redireciona já com sessionId + userId
            return RedirectToAction(
                "Index",
                "Denuncia",
                new { sessionId, userId = user.Id }
            );
        }

        [HttpPost("Logout")]
        public IActionResult Logout(string sessionId)
        {
            if (!string.IsNullOrWhiteSpace(sessionId))
                _cache.Remove(sessionId);

            return RedirectToAction(nameof(Index));
        }

        
        private bool SessaoValida(string sessionId)
        {
            return !string.IsNullOrWhiteSpace(sessionId)
                   && _cache.TryGetValue(sessionId, out _);
        }
    }
}

