namespace HealthBlog.Services.Users
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using System.Threading.Tasks;
	using System.Linq;

	using Contracts;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Common.Users.ViewModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using System.Collections.Generic;
	using Microsoft.EntityFrameworkCore;
	using HealthBlog.Common.Trainers.BindingModels;
	using HealthBlog.Common.Exceptions;
	using HealthBlog.Services.Trainers.Contracts;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class DaysService : BaseEFService, IDaysService
	{
		private readonly IUserProgramsService userProgramsService;
		private readonly ITrainersProgramsService trainersProgramsService;
		private readonly ITrainingsService trainingsService;
		private readonly IMealsService mealsService;

		public DaysService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager,
			IUserProgramsService userProgramsService,
			ITrainersProgramsService trainersProgramsService,
			ITrainingsService trainingsService,
			IMealsService mealsService)
			: base(dbContext, mapper, userManager)
		{
			this.userProgramsService = userProgramsService;
			this.trainersProgramsService = trainersProgramsService;
			this.trainingsService = trainingsService;
			this.mealsService = mealsService;
		}

		public async Task<int> CreateDayAsync(string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var program = await this.userProgramsService.GetDefaulttUserProgram(user.Id);

			var day = new Day()
			{
				AuthorId = user.Id
			};
			this.DbContext.Days.Add(day);

			program.Days.Add(new ProgramDay()
			{
				Day = day
			});

			await this.DbContext.SaveChangesAsync();

			return day.Id;
		}

		public async Task<DayDetailsViewModel> GetDayDetailsByIdAsync(int id, string username)
		{
			var userId = (await this.GetUserByNamedAsync(username))?.Id;

			var defaultProgramId = (await this.userProgramsService.GetDefaulttUserProgram(userId)).Id;

			var ownedProgramIds = (await this.DbContext.Users
				.Include(u => u.OwnedPrograms)
				.FirstOrDefaultAsync(u => u.Id == userId))
					.OwnedPrograms
					.Select(op => op.ProgramId)
					.ToList();

			var defaultprogramDay = await this.DbContext.ProgramDays
				.Include(pd => pd.Day.Author)
				.Include(pd => pd.Day.Trainings)
					.ThenInclude(td => td.Training)
				.Include(pd => pd.Day.Meals)
					.ThenInclude(md => md.Meal)
				.FirstOrDefaultAsync(pd => pd.DayId == id && (pd.ProgramId == defaultProgramId || ownedProgramIds.Any(opId => opId == pd.ProgramId)));

			if (defaultprogramDay == null)
			{
				throw new InvalidDayException();
			}

			var model = this.Mapper.Map<DayDetailsViewModel>(defaultprogramDay);
			model.IsCreatedByCurrentUser = defaultprogramDay.Day.Author.UserName == username;

			return model;
		}

		public async Task<IEnumerable<AllDaysViewModel>> GetAllDaysAsync(string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var defaultDays = (await this.userProgramsService.GetDefaulttUserProgram(user.Id)).Days;

			var days = this.Mapper.Map<IEnumerable<AllDaysViewModel>>(
				await this.DbContext.Days
					.Where(d => defaultDays
						.Any(dd => dd.DayId == d.Id))
					.Include(d => d.Meals)
					.Include(d => d.Trainings)
					.ToListAsync());

			return days;
		}

		public async Task AddTrainingToDayAsync(int dayId, int trainingId, string username)
		{
			var day = await this.GetDayAsync(dayId, username);

			var training = await this.trainingsService.GetTrainingAsync(trainingId, username);

			day.Trainings.Add(new TrainingDay()
			{
				Training = training,
			});

			await this.DbContext.SaveChangesAsync();
		}

		public async Task AddMealToDayAsync(int dayId, int mealId, string username)
		{
			var day = await this.GetDayAsync(dayId, username);

			var meal = await this.mealsService.GetMealAsync(mealId, username);

			day.Meals.Add(new MealDay()
			{
				Meal = meal,
			});

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<Day> GetDayAsync(int id, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var defaultProgram = await this.userProgramsService.GetDefaulttUserProgram(user.Id);

			var day = defaultProgram.Days
					.FirstOrDefault(d => d.DayId == id)?
					.Day;

			if (day == null)
			{
				throw new InvalidDayException();
			}

			return day;
		}

		public async Task<AddTrainingToDayModel> GetDayTrainingsByIdAsync(int dayId, string username)
		{
			var trainings = await this.trainingsService.GetAllUserTrainingsAsync(username);

			var model = new AddTrainingToDayModel()
			{
				Id = dayId,
				Trainings = trainings.Select(t => new SelectListItem(t.Name, t.Id.ToString()))
			};

			return model;
		}

		public async Task<AddMealToDayModel> GetDayMealsByIdAsync(int id, string username)
		{
			var meals = await this.mealsService.GetAllUserMealsAsync(username);

			var model = new AddMealToDayModel()
			{
				Id = id,
				Meals = meals.Select(m => new SelectListItem(m.Name, m.Id.ToString()))
			};

			return model;
		}

		public async Task RemoveTrainingFromDayAsync(int dayId, int trainingId, string username)
		{
			var day = await this.GetDayAsync(dayId, username);

			var training = await this.trainingsService.GetTrainingAsync(trainingId, username);

			var trainingDay = this.DbContext.TrainingDays
				.FirstOrDefault(td => td.TrainingId == trainingId && td.DayId == dayId);

			if (trainingDay != null)
			{
				day.Trainings.Remove(trainingDay);

				await this.DbContext.SaveChangesAsync();
			}
			else
			{
				throw new InvalidTrainingDayException();
			}
		}

		public async Task RemoveMealFromDayAsync(int dayId, int mealId, string username)
		{
			var day = await this.GetDayAsync(dayId, username);

			var meal = await this.mealsService.GetMealAsync(mealId, username);

			var mealDay = await this.DbContext.MealDays
				.FirstOrDefaultAsync(md => md.MealId == meal.Id && md.DayId == day.Id);

			if (meal != null)
			{
				day.Meals.Remove(mealDay);
				await this.DbContext.SaveChangesAsync();
			}
			else
			{
				throw new InvalidMealDayException();
			}
		}

		public async Task DeleteDayAsync(int dayId, string username)
		{
			var day = await this.GetDayAsync(dayId, username);

			this.DbContext.Days.Remove(day);

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<AddDayToProgramBindingModel> GetAddToProgramModelAsync(string username, int dayId)
		{
			var day = await this.DbContext.Days
				.Include(d => d.Programs)
				.FirstOrDefaultAsync(d => d.Id == dayId);

			var programs = (await this.trainersProgramsService.GetAllProgramsForAdding(username))
				.Where(p => !day.Programs
					.Any(dp => dp.ProgramId == int.Parse(p.Value)));

			var model = new AddDayToProgramBindingModel()
			{
				DayId = dayId,
				Programs = programs
			};

			return model;
		}

		public async Task AddDayToProgramAsync(int programId, int dayId, string username)
		{
			var day = await this.GetDayAsync(dayId, username);

			var program = await this.trainersProgramsService.GetProgramByIdAsync(programId, username);

			this.DbContext.ProgramDays.Add(new ProgramDay()
			{
				ProgramId = programId,
				DayId = dayId
			});

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<bool> IsCreatorUserAsync(string username, int id)
		{
			string userId = (await this.GetUserByNamedAsync(username)).Id;

			return await this.DbContext.Days
				.AnyAsync(d => d.AuthorId == userId && d.Id == id);
		}
	}
}
