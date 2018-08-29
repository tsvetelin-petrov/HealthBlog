namespace HealthBlog.Services.Users
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using System.Threading.Tasks;

	using Contracts;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using HealthBlog.Common.Users.ViewModels;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using System;
	using HealthBlog.Common.Exceptions;

	public class MealsService : BaseEFService, IMealsService
	{
		public MealsService(
			HealthBlogDbContext dbContext, 
			IMapper mapper,
			UserManager<User> userManager) 
			: base(dbContext, mapper, userManager)
		{
		}

		public async Task CreateMealAsync(MealCreateBindingModel model, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			if (model == null)
			{
				throw new ArgumentNullException();
			}

			user.Meals.Add(this.Mapper.Map<Meal>(model));

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<AllMealsViewModel>> GetAllMealDetailssAsync(string username)
		{
			var meals = this.Mapper.Map<IEnumerable<AllMealsViewModel>>(
				await this.GetAllUserMealsAsync(username));

			return meals;
		}

		public async Task<IEnumerable<Meal>> GetAllUserMealsAsync(string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var meals = this.DbContext.Meals
					.Where(m => m.UserId == user.Id);

			return meals;
		}

		public async Task<Meal> GetMealAsync(int id, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var meal = await this.DbContext.Meals
				.Include(m => m.User)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (meal == null)
			{
				throw new InvalidMealException();
			}

			if (!await this.IsValidMealAsync(id, username))
			{
				throw new ValidationMealException();
			}

			return meal;
		}

		public async Task<MealDetailsViewModel> GetMealDetailsAsync(int id, string username)
		{
			var meal = await this.GetMealAsync(id, username);

			var model = this.Mapper.Map<MealDetailsViewModel>(
				meal);
			model.IsCreatedByCurrentUser = meal.User.UserName == username;

			return model;
		}

		private async Task<bool> IsValidMealAsync(int mealId, string username)
		{
			var user = await this.DbContext.Users
				.Include(u => u.OwnedPrograms)
					.ThenInclude(op => op.Program.Days)
						.ThenInclude(d => d.Day.Meals)
				.Include(u => u.Meals)
				.FirstOrDefaultAsync(u => u.UserName == username);

			bool isInCreatedPrograms = user.Meals.Any(m => m.Id == mealId);

			if (isInCreatedPrograms)
				return true;

			return user.OwnedPrograms
				.Any(op => op.Program.Days
					.Any(d => d.Day.Meals
						.Any(t => t.MealId == mealId)));
		}
	}
}
