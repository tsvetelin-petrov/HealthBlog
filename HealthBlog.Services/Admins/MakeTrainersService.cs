namespace HealthBlog.Services.Admins
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	using Contracts;
	using HealthBlog.Common.Admins.ViewModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using HealthBlog.Common.Exceptions;

	public class MakeTrainersService : BaseEFService, IMakeTrainersService
	{
		private const string trainerRole = "Trainer";

		public MakeTrainersService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager)
			: base(dbContext, mapper, userManager)
		{
		}

		public async Task<IEnumerable<AllTrainerRequestsViewModel>> GetAllTrainerRequestsAsync()
		{
			var users = this.Mapper.Map<IEnumerable<AllTrainerRequestsViewModel>>(
				await this.DbContext.Users
					.Where(user => !string.IsNullOrWhiteSpace(user.CertificatePath) && !user.IsResponded)
					.ToListAsync());

			return users;
		}

		public async Task MakeTrainerAsync(string userId)
		{
			var user = await this.UserManager.FindByIdAsync(userId);
			user.IsResponded = true;

			await this.UserManager.AddToRoleAsync(user, trainerRole);
		}

		public async Task DeleteCertificateAsync(string userId)
		{
			var user = await this.UserManager.FindByIdAsync(userId);

			if (File.Exists(user.CertificatePath))
			{
				File.Delete(user.CertificatePath);
			}
			else
			{
				throw new InvalidCertificateException();
			}
			user.IsResponded = false;
			user.CertificatePath = string.Empty;
			await this.DbContext.SaveChangesAsync();
		}
	}
}
