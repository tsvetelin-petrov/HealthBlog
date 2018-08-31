namespace HealthBlog.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Error()
        {
			string message = 
				this.TempData.ContainsKey("Message") 
				? this.TempData["Message"].ToString() : null;

			this.ViewData["message"] = string.IsNullOrWhiteSpace(message)
				? "Unhadled"
				: message;

			return View();
        }
    }
}
