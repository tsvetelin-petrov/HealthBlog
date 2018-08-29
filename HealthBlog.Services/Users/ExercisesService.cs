namespace HealthBlog.Services.Users
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Contracts;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using Microsoft.EntityFrameworkCore;
	using System;
	using HealthBlog.Common.Exceptions;

	public class ExercisesService : BaseEFService, IExercisesService
	{
		private readonly UserManager<User> userManager;

		public ExercisesService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager)
			: base(dbContext, mapper, userManager)
		{
			this.userManager = userManager;
		}

		public async Task CreateExerciseAsync(ExerciseCreateBindingModel model, string username)
		{
			if (model == null)
			{
				throw new ArgumentNullException();
			}

			var user = await this.GetUserByNamedAsync(username);

			var exercise = this.Mapper.Map<Exercise>(model);

			user.Exercises.Add(exercise);

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<AllExercisesViewModel>> GetAllExercisesAsync(string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var exercises = this.Mapper.Map<IEnumerable<AllExercisesViewModel>>(
				this.DbContext.Exercises
					.Where(e => e.UserId == user.Id));

			return exercises;
		}

		public async Task<ExerciseDetailsViewModel> GetExerciseDetailsAsync(int id, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var exercise = this.Mapper.Map<ExerciseDetailsViewModel>(await this.DbContext.Exercises
				.FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id));

			return exercise;
		}

		public async Task DeleteExerciseAsync(int id, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var exercise = await this.GetExerciseAsync(id, user.Id);

			this.DbContext.Exercises.Remove(exercise);

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<Exercise> GetExerciseAsync(int id, string userId)
		{
			var exercise = await this.DbContext.Exercises.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

			if (exercise == null)
			{
				throw new InvalidExerciseException();
			}

			return exercise;
		}
	}
}
