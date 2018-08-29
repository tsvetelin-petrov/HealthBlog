namespace HealthBlog.Web.Pages.Meals
{
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Common.Constants;
	using HealthBlog.Common.Users.ViewModels;

	public class DetailsModel : BaseMealsModel
    {
		public DetailsModel(
			IMealsService mealsService) 
			: base(mealsService)
		{
		}

		public MealDetailsViewModel Meal { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
        {
			this.Meal = await this.MealsService.GetMealDetailsAsync(id, this.User.Identity.Name);

			if (this.Meal == null)
			{
				return this.RedirectToPage(PageConstants.MealsIndex);
			}

			return this.Page();
        }
    }
}