using HealthBlog.Common.Trainers.BindingModels;
using System.Threading.Tasks;

namespace HealthBlog.Services.Contracts
{
	public interface ICreateProgram
	{
		Task CreateProgramAsync(ProgramCreateBindingModel model, string username);
	}
}
