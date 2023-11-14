using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class ClienteController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
