namespace HealthBlog.Web.Areas.Administrator.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	using HealthBlog.Services.Admins.Contracts;
	using HealthBlog.Common.Constants;

	[Authorize(Roles = RolesConstants.Administrator)]
	[Area(RolesConstants.Administrator)]
	public class MakeTrainerController : Controller
    {
		private readonly IMakeTrainersService confirmsService;

		public MakeTrainerController(
			IMakeTrainersService confirmsService)
		{
			this.confirmsService = confirmsService;
		}

		public async Task<IActionResult> Index()
		{
			var model = await this.confirmsService.GetAllTrainerRequestsAsync();

			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Confirm(string id)
		{
			await this.confirmsService.MakeTrainerAsync(id);

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.MakeTrainer, new { area = RolesConstants.Administrator});
		}

		[HttpPost]
		public async Task<IActionResult> Decline(string id)
		{
			await this.confirmsService.DeleteCertificateAsync(id);

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.MakeTrainer, new { area = RolesConstants.Administrator });

		}
	}
}
