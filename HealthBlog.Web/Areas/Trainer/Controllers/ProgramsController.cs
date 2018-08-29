namespace HealthBlog.Web.Areas.Trainer.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	using HealthBlog.Common.Trainers.BindingModels;
	using HealthBlog.Services.Trainers.Contracts;
	using HealthBlog.Common.Constants;
	using HealthBlog.Web.Filters;
	using HealthBlog.Common.Users.BindingModels;

	[Area(RolesConstants.Trainer)]
	[Authorize(Roles = RolesConstants.Trainer)]
	public class ProgramsController : Controller
	{
		private readonly ITrainersProgramsService programsService;

		public ProgramsController(
			ITrainersProgramsService programsService)
		{
			this.programsService = programsService;
		}

		[HttpGet]
		public IActionResult Create()
		{
			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProgramCreateBindingModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View();
			}

			await this.programsService.CreateProgramAsync(model, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Programs, new { area = RolesConstants.Trainer });
		}

		[HttpPost]
		[ServiceFilter(typeof(TrainerProgramCreatorFilter))]
		public async Task<IActionResult> DeleteDay(int programId, int dayId)
		{
			await this.programsService.DeleteDayAsync(dayId, programId, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Programs, new { id = programId, area = ""});
		}

		[HttpGet]
		[ServiceFilter(typeof(TrainerProgramCreatorFilter))]
		public async Task<IActionResult> Sell(int id)
		{
			var model = await this.programsService.GetProgramForSelling(id, this.User.Identity.Name);

			return this.View(model);
		}

		[HttpPost]
		[ServiceFilter(typeof(TrainerProgramCreatorFilter))]
		public async Task<IActionResult> Sell(int id, ProgramSellBindingModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return await this.Sell(id);
			}

			await this.programsService.SellProgramAsync(id, model, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Programs, new { area = RolesConstants.Trainer });
		}

		public async Task<IActionResult> Index()
		{
			var model = await this.programsService.GetAllOwnedProgramsForIndexAsync(this.User.Identity.Name);

			return this.View(model);
		}

	}
}
