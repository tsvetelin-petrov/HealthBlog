using System.Collections.Generic;
using System.Threading.Tasks;
using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Common.Users.ViewModels;

namespace HealthBlog.Services.Users.Contracts
{
	public interface IExercisesService
	{
		Task CreateExerciseAsync(ExerciseCreateBindingModel model, string username);

		Task<IEnumerable<AllExercisesViewModel>> GetAllExercisesAsync(string username);

		Task<ExerciseDetailsViewModel> GetExerciseDetailsAsync(int id, string username);

		Task DeleteExerciseAsync(int id, string username);
	}
}