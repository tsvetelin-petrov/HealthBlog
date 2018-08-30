namespace HealthBlog.Services.Trainers.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using HealthBlog.Common.Trainers.BindingModels;
	using HealthBlog.Common.Trainers.ViewModels;
	using HealthBlog.Models;
	using HealthBlog.Services.Contracts;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public interface ITrainersProgramsService : ICreateProgram
	{
		Task DeleteDayAsync(int dayId, int programId, string username);

		Task<IEnumerable<AllProgramsViewModel>> GetAllOwnedProgramsForIndexAsync(string username);

		Task<Program> GetProgramByIdAsync(int programId, string username);

		Task SellProgramAsync(int id, ProgramSellBindingModel model, string username);

		Task<bool> IsCreatorUserAsync(string username, int id);

		Task<IEnumerable<SelectListItem>> GetAllProgramsForAdding(string username);

		Task<ProgramSellBindingModel> GetProgramForSelling(int id, string username);
	}
}