using System.Collections.Generic;
using System.Threading.Tasks;
using HealthBlog.Common.Trainers.BindingModels;
using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Common.Users.ViewModels;
using HealthBlog.Models;

namespace HealthBlog.Services.Users.Contracts
{
	public interface IDaysService
	{
		Task AddMealToDayAsync(int dayId, int mealId, string username);

		Task AddTrainingToDayAsync(int dayId, int trainingId, string username);

		Task<int> CreateDayAsync(string username);

		Task<IEnumerable<AllDaysViewModel>> GetAllDaysAsync(string username);

		Task<Day> GetDayAsync(int id, string username);

		Task<DayDetailsViewModel> GetDayDetailsByIdAsync(int id, string username);

		Task<AddMealToDayModel> GetDayMealsByIdAsync(int id, string username);

		Task<bool> IsCreatorUserAsync(string username, int id);

		Task<AddTrainingToDayModel> GetDayTrainingsByIdAsync(int dayId, string username);

		Task RemoveMealFromDayAsync(int dayId, int mealId, string username);

		Task RemoveTrainingFromDayAsync(int dayId, int trainingId, string username);

		Task DeleteDayAsync(int dayId, string username);

		Task<AddDayToProgramBindingModel> GetAddToProgramModelAsync(string name, int dayId);

		Task AddDayToProgramAsync(int programId, int dayId, string name);
	}
}