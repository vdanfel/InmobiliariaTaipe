using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class DashboardController:Controller
    {
        public IActionResult Index() 
        { 
            DashboardController dashboardController = new DashboardController();

            return View();
        }
    }
}
