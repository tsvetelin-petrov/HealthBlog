namespace HealthBlog.Services.Users.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Models;
	using HealthBlog.Services.Contracts;

	public interface IUserProgramsService : ICreateProgram
	{
		Task BuyProgramAsync(int programId, string username);

		Task<IEnumerable<ProgramForBuyingVewModel>> GetProgramsForBuyingAsync(string username, string searchTerm);

		Task<ICollection<ProgramsIndexViewModel>> GetOwnedAndCreatedProgramsAsync(string username);

		Task<ProgramDetailsViewModel> GetProgramDetailsAsync(string name, int id);

		Task<Program> GetOrCreateDefaulttUserProgram(string userId);
	}
}