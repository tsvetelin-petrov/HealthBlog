using System.Threading.Tasks;
using HealthBlog.Common.Trainers.BindingModels;
using Microsoft.AspNetCore.Http;

namespace HealthBlog.Services.Trainers.Contracts
{
	public interface ITrainerValidationService
	{
		Task SubmitCertificatesAsync(IFormFile model, string username);
	}
}