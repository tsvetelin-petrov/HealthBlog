namespace HealthBlog.Web.Pages.Meals
{
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Services.Users.Contracts;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;
	using HealthBlog.Common.Constants;

	public class MealsCreateModel : BaseMealsModel
    {
		public MealsCreateModel(
			IMealsService mealsService) 
			: base(mealsService)
		{
		}

		[BindProperty]
		public MealCreateBindingModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
			if (!this.ModelState.IsValid)
			{
				return this.Page();
			}

			await this.MealsService.CreateMealAsync(this.Input, this.User.Identity.Name);

			return this.RedirectToPage(PageConstants.MealsIndex);
        }
    }
}