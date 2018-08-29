namespace HealthBlog.Services.Users
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	using Contracts;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using System.Collections.Generic;
	using Microsoft.EntityFrameworkCore;
	using HealthBlog.Common.Exceptions;

	public class TrainingsService : BaseEFService, ITrainingsService
	{
		private readonly IExercisesService exercisesService;

		public TrainingsService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager,
			IExercisesService exercisesService) 
			: base(dbContext, mapper, userManager)
		{
			this.exercisesService = exercisesService;
		}

		public async Task<int> CreateTrainingAsync(TrainingCreateBindingModel model, string username)
		{
			if (model == null)
			{
				throw new ArgumentNullException();
			}

			var user = await this.GetUserByNamedAsync(username);

			var training = this.Mapper.Map<Training>(model);
			training.UserId = user.Id;

			this.DbContext.Trainings.Add(training);

			await this.DbContext.SaveChangesAsync();

			return training.Id;
		}

		public async Task AddExerciseToTrainingAsync(TrainingExerciseInput model, int id, string username)
		{
			if (model == null)
			{
				throw new ArgumentNullException();
			}

			var user = await this.GetUserByNamedAsync(username);

			var training = await this.GetTrainingAsync(id, username);

			if (!this.DbContext.Exercises
				.Where(e => e.UserId == user.Id)
				.Any(ex => ex.Id == model.ExerciseId))
			{
				throw new InvalidExerciseException();
			}

			training.Exercises.Add(this.Mapper.Map<TrainingExercise>(model));
			await this.DbContext.SaveChangesAsync();
		}

		public async Task<TrainingDetailsViewModel> GetTrainingDetailsAsync(string username, int trainingId)
		{
			var user = await this.GetUserByNamedAsync(username);

			if (!await this.IsValidTrainingAsync(trainingId, username))
			{
				throw new InvalidTrainingException();
			}

			var training = this.Mapper
				.Map<TrainingDetailsViewModel>(
					await this.DbContext.Trainings
						.Include(t => t.Exercises)
							.ThenInclude(te => te.Exercise)
						.FirstAsync(t => t.Id == trainingId));

			training.IsCreatedByCurrentUser = (await this.DbContext.Trainings.FindAsync(trainingId)).UserId == user.Id;

			return training;
		}

		public async Task<IEnumerable<AllTrainingsViewModel>> GetAllUserTrainingViewModelsAsync(string username)
		{
			var trainings = this.Mapper.Map<IEnumerable<AllTrainingsViewModel>>(
				await this.GetAllUserTrainingsAsync(username));

			return trainings;
		}

		public async Task<bool> IsValidTrainingAsync(int trainingId, string username)
		{
			var user = await this.DbContext.Users
				.Include(u => u.OwnedPrograms)
					.ThenInclude(op => op.Program.Days)
						.ThenInclude(d => d.Day.Trainings)
				.Include(u => u.Trainings)
				.FirstOrDefaultAsync(u => u.UserName == username);

			bool isCreatedTraining = user.Trainings
				.Any(t => t.Id == trainingId);

			if (isCreatedTraining)
				return true;

			return user.OwnedPrograms
				.Any(op => op.Program.Days
					.Any(d => d.Day.Trainings
						.Any(t => t.TrainingId == trainingId)));
		}

		public async Task<TrainingExerciseModel> GetTrainingExercisesByIdAsync(int id, string username)
		{
			var training = await this.GetTrainingAsync(id, username);
			var trainingExercise = new TrainingExerciseModel()
			{
				TrainingName = training.Name
			};

			trainingExercise.Exercises = await this.exercisesService.GetAllExercisesAsync(username);

			return trainingExercise;
		}

		public async Task DeleteTrainingAsync(int exerciseId, int trainingId, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var trainingExercise = this.DbContext.TrainingExercises
				.FirstOrDefault(te => te.ExerciseId == exerciseId && te.TrainingId == trainingId);

			if (trainingExercise == null)
			{
				throw new InvalidTrainingExerciseException();
			}

			this.DbContext.TrainingExercises
				.Remove(trainingExercise);

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<Training> GetTrainingAsync(int id, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var training = await this.DbContext.Trainings.FirstOrDefaultAsync(t => t.Id == id);

			if (training == null)
			{
				throw new InvalidTrainingException();
			}

			if (!await this.IsValidTrainingAsync(id, username))
			{
				throw new InvalidTrainingException();
			}


			return training;
		}

		public async Task<IEnumerable<Training>> GetAllUserTrainingsAsync(string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var trainings = this.DbContext.Trainings
					.Include(t => t.Exercises)
					.Where(t => t.UserId == user.Id);

			return trainings;
		}

		public async Task<bool> IsCreatorUserAsync(string username, int id)
		{
			var userId = (await this.GetUserByNamedAsync(username)).Id;

			return await this.DbContext.Trainings
				.AnyAsync(t => t.UserId == userId && t.Id == id);
		}
	}
}
