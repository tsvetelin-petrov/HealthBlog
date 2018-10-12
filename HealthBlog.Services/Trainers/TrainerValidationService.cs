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
	using HealthBlog.Common;

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
			CoreValidator.ThrowIfNull(cerificate);
			ThrowIfNotImage(cerificate);

			var user = await this.GetUserByNamedAsync(username);
			ThrowIfInvalidUser(user);

			await UploadFileAsync(cerificate, username, user);

			user.CertificateUploadTimes++;
			await this.DbContext.SaveChangesAsync();
		}

		private async Task UploadFileAsync(IFormFile cerificate, string username, User user)
		{
			//TODO: Upload to drive
			user.CertificatePath = $"images/Certificates/{username}-{cerificate.FileName}";
			FileStream fileStream = GetPath(user);
			await cerificate.CopyToAsync(fileStream);
		}

		private FileStream GetPath(User user)
		{
			var pathString = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot", user.CertificatePath);
			FileStream fileStream = new FileStream(pathString, FileMode.Create);
			return fileStream;
		}

		private void ThrowIfInvalidUser(User user)
		{
			if (user.CertificateUploadTimes >= 3)
			{
				throw new CertificateUploadTimesException();
			}
		}

		private void ThrowIfNotImage(IFormFile cerificate)
		{
			if (!cerificate.ContentType.ToLower().StartsWith("image"))
			{
				throw new InvalidCertificateUploadException();
			}
		}
	}
}
