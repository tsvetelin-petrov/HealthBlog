namespace HealthBlog.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Services.Users.Contracts;
	using System;
	using HealthBlog.Common.Constants;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Web.Filters;

	[Authorize]
	public class TrainingsController : Controller
	{
		private readonly ITrainingsService trainingsService;

		public TrainingsController(
			ITrainingsService trainingsService)
		{
			this.trainingsService = trainingsService;
		}

		public async Task<IActionResult> Index()
		{
			var trainings = await this.trainingsService
				.GetAllUserTrainingViewModelsAsync(this.User.Identity.Name);

			return this.View(trainings);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(TrainingCreateBindingModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View();
			}

			var trainingId = await this.trainingsService.CreateTrainingAsync(model, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Trainings, new { id = trainingId });
		}

		public async Task<IActionResult> Details(int id)
		{
			var training = await this.trainingsService.GetTrainingDetailsAsync(this.User.Identity.Name, id);

			if (training == null)
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Trainings);
			}

			return this.View(training);
		}

		[HttpGet]
		[ServiceFilter(typeof(TrainingCreatorFilter))]
		public async Task<IActionResult> AddExercise(int id)
		{
			if (!await this.trainingsService.IsValidTrainingAsync(id, this.User.Identity.Name))
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Trainings);
			}

			var model = await this.trainingsService.GetTrainingExercisesByIdAsync(id, this.User.Identity.Name);

			this.ViewData["id"] = id;

			return this.View(model);
		}

		[HttpPost]
		[ServiceFilter(typeof(TrainingCreatorFilter))]
		public async Task<IActionResult> AddExercise(int id, TrainingExerciseModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View();
			}

			try
			{
				await this.trainingsService.AddExerciseToTrainingAsync(model.Input, id, this.User.Identity.Name);
			}
			catch (Exception)
			{
				//TODO: Add constants.
				this.ModelState.AddModelError(string.Empty, "Упражнението вече е добавено.");

				return await this.AddExercise(id);
			}

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Trainings, new { id });
		}

		[HttpPost]
		[ServiceFilter(typeof(TrainingCreatorFilter))]
		public async Task<IActionResult> DeleteExercise(int trainingId, int exerciseId)
		{
			await this.trainingsService.DeleteTrainingAsync(exerciseId, trainingId, this.User.Identity.Name);

			return this.RedirectToAction(ActionConstants.Details, ControllerConstants.Trainings, new { id = trainingId });
		}
	}
}
