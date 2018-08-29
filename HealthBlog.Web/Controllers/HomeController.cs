namespace HealthBlog.Web.Controllers
{
	using System.Diagnostics;
	using Microsoft.AspNetCore.Mvc;

	using HealthBlog.Common.Users.ViewModels;

	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
			return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message = null)
        {
			this.ViewData["message"] = string.IsNullOrWhiteSpace(message)
				? "Unhadled"
				: message;

            return View();
        }
    }
}
