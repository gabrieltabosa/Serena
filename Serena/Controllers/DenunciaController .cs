using Microsoft.AspNetCore.Mvc;
using Serena.Models;
using Serena.Models.DTOs;
using Serena.Service;
using System.Net;

namespace Serena.Controllers
{
    // O controlador principal pode ter um prefixo de rota, mas para views e partials 
    // é comum usar a rota padrão ou apenas a ação.
    public class DenunciaController : Controller
    {
        private readonly IDenunciaService _denunciaService;

        public DenunciaController(IDenunciaService denunciaService)
        {
            _denunciaService = denunciaService;
        }

        // GET /Denuncia/UserReports/{userId}
        /// <summary>
        /// Busca todas as denúncias de um usuário e retorna a Partial View.
        /// Este é um cenário comum onde o front-end solicita um bloco de HTML.
        /// </summary>
        /// <param name="userId">ID do usuário logado.</param>
        [HttpGet]
        public async Task<IActionResult> UserReports(int userId)
        {
            try
            {
                // **ATENÇÃO: Este método exige a correção mencionada na seção 1.C**
                var denuncias = await _denunciaService.GetAllByUserAsync(userId);

                // Retorna a Partial View com a lista de denúncias como modelo
                return PartialView("_denunciaPartial", denuncias);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Se a API retornar 404, retorna uma Partial View vazia ou com uma mensagem de erro
                ViewBag.ErrorMessage = "Nenhuma denúncia encontrada para este usuário.";
                return PartialView("_denunciaPartial", Enumerable.Empty<DenunciaViewModel>());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar as denúncias.";
                // Retorna 500 ou um erro
                return StatusCode(500, "Erro interno ao processar a solicitação.");
            }
        }

        // POST /Denuncia/Create
        /// <summary>
        /// Processa o formulário de criação de denúncia.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Recomendado para formulários MVC
        public async Task<IActionResult> Create([FromForm] DenunciaDto denuncia)
        {
            if (!ModelState.IsValid)
            {
                // Se o modelo não for válido, retorna o formulário com erros
                return View(denuncia); // Assumindo que a view principal é 'Create'
            }

            try
            {
                var createdDenuncia = await _denunciaService.CreateAsync(denuncia);

                // Após a criação bem-sucedida, redireciona o usuário
                // Exemplo: Redirecionar para a lista de relatórios
                return RedirectToAction(nameof(UserReports), new { userId = createdDenuncia.UserId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao registrar a denúncia: " + ex.Message);
                return View(denuncia);
            }
        }
    }
}
