namespace HealthBlog.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Common.Constants;

	[Authorize]
	public class ProgramsController : Controller
    {
		private readonly IUserProgramsService programsService;

		public ProgramsController(
			IUserProgramsService programsService)
		{
			this.programsService = programsService;
		}

		public async Task<IActionResult> Index()
		{
			var model = await this.programsService.GetOwnedAndCreatedProgramsAsync(this.User.Identity.Name);

			return this.View(model);
		}

		public async Task<IActionResult> Search(string searchTerm = null)
		{
			var model = await this.programsService.GetProgramsForBuyingAsync(this.User.Identity.Name, searchTerm);

			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Buy(int id)
		{
			await this.programsService.BuyProgramAsync(id, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Programs);
		}

		public async Task<IActionResult> Details(int id)
		{
			var program = await this.programsService.GetProgramDetailsAsync(this.User.Identity.Name, id);

			if (program == null)
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Programs, new { area = RolesConstants.Trainer });
			}

			return this.View(program);
		}
	}
}
