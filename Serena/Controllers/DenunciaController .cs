using Microsoft.AspNetCore.Mvc;
using Serena.Models.DTOs;
using Serena.Models;
using Serena.Service;

namespace Serena.Controllers
{
    public class DenunciaController: Controller
    {
        private readonly IDenunciaService _denunciaService;

        public DenunciaController(IDenunciaService denunciaService)
        {
            _denunciaService = denunciaService;
        }

        public async Task<IActionResult> Index(int userId)
        {
            if (userId <= 0) return BadRequest();
            var list = await _denunciaService.GetAllByUserAsync(userId);
            return View(list);
        }

        public IActionResult Create() => View(new DenunciaDto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DenunciaDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var created = await _denunciaService.CreateAsync(model);
                return RedirectToAction(nameof(Details), new { id = created.Id });
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Erro ao comunicar com o servidor. Tente novamente.");
                return View(model);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _denunciaService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }
    }
}
