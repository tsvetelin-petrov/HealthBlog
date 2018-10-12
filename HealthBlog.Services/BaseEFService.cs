namespace HealthBlog.Services
{
	using Microsoft.AspNetCore.Identity;
	using System;
	using System.Threading.Tasks;

	using AutoMapper;
	using Data;
	using HealthBlog.Models;
	using HealthBlog.Common.Exceptions;
	using HealthBlog.Common;

	public abstract class BaseEFService
    {
		protected BaseEFService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager)
		{
			this.DbContext = dbContext;
			this.Mapper = mapper;
			this.UserManager = userManager;
		}

		protected HealthBlogDbContext DbContext { get; private set; }

		protected IMapper Mapper { get; private set; }

		protected UserManager<User> UserManager { get; private set; }

		protected async Task<User> GetUserByNamedAsync(string name)
		{
			var user = await this.UserManager.FindByNameAsync(name);

			CoreValidator.ThrowIfNull(user, new InvalidUserException());

			return user;
		}
	}
}
