using Microsoft.AspNetCore.Mvc;

namespace HealthGuide.API.Appointments.Controllers
{
    public class HomeController : Controller 
    {        
        public IActionResult Index()
        {
            return View();
        }
    }
}