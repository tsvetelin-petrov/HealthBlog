using System.Collections.Generic;
using System.Threading.Tasks;
using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Common.Users.ViewModels;
using HealthBlog.Models;

namespace HealthBlog.Services.Users.Contracts
{
	public interface ITrainingsService
	{
		Task AddExerciseToTrainingAsync(TrainingExerciseInput model, int id, string username);

		Task<int> CreateTrainingAsync(TrainingCreateBindingModel model, string username);

		Task<TrainingDetailsViewModel> GetTrainingDetailsAsync(string username, int trainingId);

		Task<IEnumerable<AllTrainingsViewModel>> GetAllUserTrainingViewModelsAsync(string username);

		Task<bool> IsValidTrainingAsync(int trainingId, string username);

		Task<TrainingExerciseModel> GetTrainingExercisesByIdAsync(int id, string username);

		Task DeleteTrainingAsync(int exerciseId, int trainingId, string username);

		Task<Training> GetTrainingAsync(int id, string username);

		Task<IEnumerable<Training>> GetAllUserTrainingsAsync(string username);

		Task<bool> IsCreatorUserAsync(string username, int id);
	}
}