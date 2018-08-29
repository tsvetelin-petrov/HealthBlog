namespace HealthBlog.Web.Pages.Meals
{
	using HealthBlog.Services.Users.Contracts;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	[Authorize]
	public abstract class BaseMealsModel : PageModel
	{
		public IMealsService MealsService { get; set; }

		public BaseMealsModel(IMealsService mealsService)
		{
			this.MealsService = mealsService;
		}
	}
}
