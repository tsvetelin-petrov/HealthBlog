namespace HealthBlog.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;
	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Common.Users.ViewModels;
	using System;
	using HealthBlog.Common.Trainers.BindingModels;
	using HealthBlog.Services.Trainers.Contracts;
	using HealthBlog.Common.Constants;

	[Authorize]
	public class DaysController : Controller
	{
		private const string alreadyAddedTraining = "Tренировката вече е добавена.";
		private const string alreadyAddedMeal = "Хрененето вече е добавена.";

		private readonly IDaysService daysService;
		private readonly ITrainingsService trainingsService;
		private readonly ITrainersProgramsService programsService;

		public DaysController(
			IDaysService daysService,
			ITrainingsService trainingsService,
			ITrainersProgramsService programsService)
		{
			this.daysService = daysService;
			this.trainingsService = trainingsService;
			this.programsService = programsService;
		}

		[HttpPost]
		public async Task<IActionResult> Create()
		{
			var dayId = await daysService.CreateDayAsync(this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Days, new { id = dayId });
		}

		public async Task<IActionResult> Details(int id)
		{
			var day = await this.daysService.GetDayDetailsByIdAsync(id, this.User.Identity.Name);

			return this.View(day);
		}

		public async Task<IActionResult> Index()
		{
			var days = await this.daysService.GetAllDaysAsync(this.User.Identity.Name);

			return this.View(days);
		}

		[HttpGet]
		public async Task<IActionResult> AddTraining(int id)
		{
			AddTrainingToDayModel model = null;
			try
			{
				model = await this.daysService.GetDayTrainingsByIdAsync(id, this.User.Identity.Name);
			}
			catch (Exception)
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Days);
			}

			this.ViewData["id"] = id;

			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddTraining(int dayId, AddTrainingToDayModel model)
		{
			try
			{
				await this.daysService.AddTrainingToDayAsync(dayId, model.Id, this.User.Identity.Name);
			}
			catch (Exception)
			{
				this.ModelState.AddModelError(string.Empty, alreadyAddedTraining);

				return await this.AddTraining(dayId);
			}

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Days, new { id = dayId });
		}

		[HttpGet]
		public async Task<IActionResult> AddMeal(int id)
		{
			AddMealToDayModel model = null;
			try
			{
				model = await this.daysService.GetDayMealsByIdAsync(id, this.User.Identity.Name);
			}
			catch (Exception)
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Days);
			}

			this.ViewData["id"] = id;

			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddMeal(int dayId, AddMealToDayModel model)
		{
			try
			{
				await this.daysService.AddMealToDayAsync(dayId, model.Id, this.User.Identity.Name);
			}
			catch (Exception)
			{
				this.ModelState.AddModelError(string.Empty, alreadyAddedMeal);

				return await this.AddTraining(dayId);
			}

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Days, new { id = dayId });
		}

		[HttpPost]
		public async Task<IActionResult> RemoveTraining(int dayId, int trainingId)
		{
			await this.daysService.RemoveTrainingFromDayAsync(dayId, trainingId, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Days, new { id = dayId });
		}

		[HttpPost]
		public async Task<IActionResult> RemoveMeal(int dayId, int mealId)
		{
			await this.daysService.RemoveMealFromDayAsync(dayId, mealId, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Days, new { id = dayId });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await this.daysService.DeleteDayAsync(id, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Days);
		}

		[HttpGet]
		[Authorize(Roles = RolesConstants.Trainer)]
		public async Task<IActionResult> AddToProgram(int id)
		{
			var model = await this.daysService.GetAddToProgramModelAsync(this.User.Identity.Name, id);

			return this.View(model);
		}

		[HttpPost]
		[Authorize(Roles = RolesConstants.Trainer)]
		public async Task<IActionResult> AddToProgram(AddDayToProgramBindingModel model, int dayId)
		{
			if (!this.ModelState.IsValid)
			{
				return await AddToProgram(dayId);
			}

			await this.daysService.AddDayToProgramAsync(model.ProgramId, dayId, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Programs, new { id = model.ProgramId });
		}
	}
}