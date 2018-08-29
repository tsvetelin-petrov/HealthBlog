namespace HealthBlog.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System;
	using System.Threading.Tasks;

	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Common.Constants;

	[Authorize]
	public class ExercisesController : Controller
	{
		private readonly IExercisesService exercisesService;

		public ExercisesController(IExercisesService exercisesService)
		{
			this.exercisesService = exercisesService;
		}

		public async Task<IActionResult> Index()
		{
			var exercises = await this.exercisesService.GetAllExercisesAsync(this.User.Identity.Name);

			return this.View(exercises);
		}

		[HttpGet]
		public IActionResult Create(string redirectUrl = null)
		{
			if (!string.IsNullOrWhiteSpace(redirectUrl))
			{
				if (Url.IsLocalUrl(redirectUrl))
				{
					this.ViewData["redirectUrl"] = redirectUrl;
				}
			}

			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ExerciseCreateBindingModel model, string redirectUrl = null)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View();
			}

			await this.exercisesService.CreateExerciseAsync(model, this.User.Identity.Name);

			if (!string.IsNullOrWhiteSpace(redirectUrl))
			{
				return Redirect(redirectUrl);
			}

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Exercises);
		}

		public async Task<IActionResult> Details(int id)
		{
			var exercise = await this.exercisesService.GetExerciseDetailsAsync(id, this.User.Identity.Name);

			if (exercise == null)
			{
				return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Exercises);
			}

			return this.View(exercise);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await this.exercisesService.DeleteExerciseAsync(id, this.User.Identity.Name);
			}
			catch (Exception)
			{
			}

			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Exercises);
		}
	}
}
