using System.Collections.Generic;
using System.Threading.Tasks;
using HealthBlog.Common.Admins.ViewModels;

namespace HealthBlog.Services.Admins.Contracts
{
	public interface IMakeTrainersService
	{
		Task DeleteCertificateAsync(string userId);

		Task<IEnumerable<AllTrainerRequestsViewModel>> GetAllTrainerRequestsAsync();

		Task MakeTrainerAsync(string userId);
	}
}