namespace HealthBlog.Web.Pages.Meals
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Services.Users.Contracts;

	public class MealsIndexModel : BaseMealsModel
    {
		public MealsIndexModel(
			IMealsService mealsService) 
			: base(mealsService)
		{
		}

		public IEnumerable<AllMealsViewModel> Meals { get; set; }

		//TODO: Add migration!
		public async Task OnGetAsync()
        {
			this.Meals = await this.MealsService.GetAllMealDetailssAsync(this.User.Identity.Name);
        }
    }
}