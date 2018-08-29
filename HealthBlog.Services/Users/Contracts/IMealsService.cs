namespace HealthBlog.Services.Users.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Models;

	public interface IMealsService
	{
		Task CreateMealAsync(MealCreateBindingModel model, string username);

		Task<IEnumerable<AllMealsViewModel>> GetAllMealDetailssAsync(string username);

		Task<MealDetailsViewModel> GetMealDetailsAsync(int id, string username);

		Task<Meal> GetMealAsync(int id, string username);

		Task<IEnumerable<Meal>> GetAllUserMealsAsync(string username);
	}
}