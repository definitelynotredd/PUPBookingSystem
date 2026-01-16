using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Import this!

namespace PUPBookingSystem.Controllers
{
    // We don't put [Authorize] here, or it locks the whole controller
    public class HomeController : Controller
    {
        // This attribute tells the app: "Let anyone see this page, even guests"
        [AllowAnonymous]
        public IActionResult Index()
        {
            // If the user is ALREADY logged in, we might want to 
            // redirect them straight to the Booking Dashboard instead of the landing page.
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Booking");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}