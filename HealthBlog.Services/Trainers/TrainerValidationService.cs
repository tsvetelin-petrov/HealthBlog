namespace HealthBlog.Services.Trainers
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Http;
	using System;
	using System.IO;
	using System.Threading.Tasks;

	using Contracts;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using HealthBlog.Common.Exceptions;

	public class TrainerValidationService : BaseEFService, ITrainerValidationService
	{
		public TrainerValidationService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager)
			: base(dbContext, mapper, userManager)
		{
		}

		public async Task SubmitCertificatesAsync(IFormFile cerificate, string username)
		{
			if (cerificate == null)
			{
				throw new ArgumentNullException();
			}

			if (!cerificate.ContentType.ToLower().StartsWith("image"))
			{
				throw new InvalidCertificateUploadException();
			}

			var user = await this.GetUserByNamedAsync(username);

			if (user.CertificateUploadTimes >= 3)
			{
				throw new CertificateUploadTimesException();
			}
			user.CertificatePath = $"images/Certificates/{username}-{cerificate.FileName}";

			var pathString = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot", user.CertificatePath);
			FileStream fileStream = new FileStream(pathString, FileMode.Create);

			await cerificate.CopyToAsync(fileStream);

			user.CertificateUploadTimes++;

			await this.DbContext.SaveChangesAsync();
		}
	}
}
