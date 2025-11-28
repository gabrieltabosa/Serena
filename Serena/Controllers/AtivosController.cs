using Microsoft.AspNetCore.Mvc;

namespace Serena.Controllers
{
    public class AtivosController: Controller
    {
        public IActionResult AdicionarAtivoPartial()
        {
            return PartialView("_AdicionarAtivoPartial");
        }

        public IActionResult ConsultaPartial()
        {
            return PartialView("_ConsultaAtivos");
        }
    }
}
