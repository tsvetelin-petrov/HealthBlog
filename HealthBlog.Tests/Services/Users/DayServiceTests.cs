using HealthBlog.Common.Exceptions;
using HealthBlog.Data;
using HealthBlog.Models;
using HealthBlog.Services.Trainers.Contracts;
using HealthBlog.Services.Users;
using HealthBlog.Services.Users.Contracts;
using HealthBlog.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBlog.Tests.Services.Users
{
	[TestClass]
	public class DayServiceTests
	{
		private const int testTrainingId = 1;
		private const int testMealId = 1;
		private const string trainingName = "training";
		private const string mealName = "meal";

		private HealthBlogDbContext dbContext;
		private DaysService service;

		[TestMethod]
		public async Task CreateDayAsync_WithoutParameters_ShouldCreteDay()
		{
			await this.service.CreateDayAsync(MockUserManager.testUsername);

			Assert.AreEqual(1, this.dbContext.Users.Find(MockUserManager.testUserId).CreatedPrograms.First().Days.Count);
		}

		[TestMethod]
		public async Task GetAllDaysAsync_WithFewDays_ShouldReturnAll()
		{
			var program = this.GetOrCreateTestProgram();

			var day1 = new Day()
			{
				AuthorId = MockUserManager.testUserId
			};
			var day2 = new Day()
			{
				AuthorId = MockUserManager.testUserId
			};
			var day3 = new Day()
			{
				AuthorId = MockUserManager.testUserId
			};

			program.Days.Add(new ProgramDay()
			{
				Day = day1
			});
			program.Days.Add(new ProgramDay()
			{
				Day = day2
			});
			program.Days.Add(new ProgramDay()
			{
				Day = day3
			});

			await this.dbContext.SaveChangesAsync();


			var days = await this.service.GetAllDaysAsync(MockUserManager.testUsername);

			Assert.AreEqual(3, days.Count());
			Assert.IsTrue(days.All(d => d != null));
		}

		[TestMethod]
		public async Task AddTrainingToDayAsync_WithValidTraining_ShouldAddIt()
		{
			var training = new Training()
			{
				Name = trainingName,
				Description = "description",
				Type = "type"
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);
			user.Trainings.Add(training);

			var day = new Day()
			{
				AuthorId = user.Id
			};

			var program = this.GetOrCreateTestProgram();
			program.Days.Add(new ProgramDay()
			{
				Day = day
			});			

			await this.dbContext.SaveChangesAsync();

			await this.service.AddTrainingToDayAsync(day.Id, testTrainingId, MockUserManager.testUsername);
			var trainingFromDb = user.CreatedPrograms.First().Days.First().Day.Trainings.First().Training;

			Assert.IsTrue(trainingFromDb.Name == trainingName);
		}

		[TestMethod]
		public async Task AddMealToDayAsync_WithValidMeal_ShouldAddIt()
		{
			var meal = new Meal()
			{
				Name = mealName,
				Description = "description",
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);
			user.Meals.Add(meal);

			var day = new Day()
			{
				AuthorId = user.Id
			};

			var program = this.GetOrCreateTestProgram();
			program.Days.Add(new ProgramDay()
			{
				Day = day
			});

			await this.dbContext.SaveChangesAsync();

			await this.service.AddMealToDayAsync(day.Id, testMealId, MockUserManager.testUsername);
			var mealFromDb = user.CreatedPrograms.First().Days.First().Day.Meals.First().Meal;

			Assert.IsTrue(mealFromDb.Name == mealName);
		}

		[TestMethod]
		public async Task GetDayAsync_WithValidParameters_ShouldReturnIt()
		{
			var day = new Day()
			{
				AuthorId =MockUserManager.testUserId
			};

			var program = this.GetOrCreateTestProgram();
			program.Days.Add(new ProgramDay()
			{
				Day = day
			});
			await this.dbContext.SaveChangesAsync();

			var dayFromService = await this.service.GetDayAsync(day.Id, MockUserManager.testUsername);

			Assert.AreSame(day, dayFromService);
		}

		[TestMethod]
		public async Task GetDayAsync_WithInvalidParameters_ShouldThrowException()
		{
			await Assert.ThrowsExceptionAsync<InvalidDayException>(() => this.service.GetDayAsync(0, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task RemoveTrainingFromDayAsync_ValidTraining_ShouldRemoveItFromTheDay()
		{
			var training = new Training()
			{
				Name = trainingName,
				Description = "description",
				Type = "type"
			};

			var user = this.dbContext.Users.Find(MockUserManager.testUserId);

			user.Trainings.Add(training);

			var program = this.GetOrCreateTestProgram();
			var day = new Day()
			{
				AuthorId = MockUserManager.testUserId
			};
			day.Trainings.Add(new TrainingDay()
			{
				Training = training
			});

			program.Days.Add(new ProgramDay()
			{
				Day = day
			});

			this.dbContext.SaveChanges();

			await this.service.RemoveTrainingFromDayAsync(day.Id, training.Id, MockUserManager.testUsername);

			Assert.IsFalse(
				this.dbContext.Users.Find(MockUserManager.testUserId)
					.CreatedPrograms.First()
						.Days.First()
							.Day.Trainings.Any());
		}

		[TestInitialize]
		public virtual void InitializeTests()
		{
			this.dbContext = MockDbContext.GetContext();
			var userManager = MockUserManager.GetUserManager(dbContext);

			var mockUserProgramService = new Mock<IUserProgramsService>();
			mockUserProgramService.Setup(opt => opt.GetDefaulttUserProgram(MockUserManager.testUserId))
				.ReturnsAsync(this.GetOrCreateTestProgram);

			var mockTrainerProgramService = new Mock<ITrainersProgramsService>();
			var mockTraininService = new Mock<ITrainingsService>();
			mockTraininService
				.Setup(opt => opt.GetTrainingAsync(testTrainingId, MockUserManager.testUsername))
				.ReturnsAsync(() => 
				{
					var training = this.dbContext.Trainings.FirstOrDefault();

					if (training == null)
					{
						throw new Exception();
					}

					return training;
				});

			var mockMealService = new Mock<IMealsService>();
			mockMealService
				.Setup(opt => opt.GetMealAsync(testMealId, MockUserManager.testUsername))
				.ReturnsAsync(() =>
				{
					var meal = this.dbContext.Meals.FirstOrDefault();

					if (meal == null)
					{
						throw new Exception();
					}

					this.dbContext.SaveChanges();

					return meal;
				});

			this.service = new DaysService(
				dbContext,
				MockAutoMapper.GetAutoMapper(),
				userManager,
				mockUserProgramService.Object,
				mockTrainerProgramService.Object,
				mockTraininService.Object,
				mockMealService.Object);
		}

		private Program GetOrCreateTestProgram()
		{
			var user = this.dbContext.Users.Find(MockUserManager.testUserId);

			if (user.CreatedPrograms.Any())
			{
				return user.CreatedPrograms.First();
			}

			var defaultProgram = new Program()
			{
				Name = "test_program_name",
				Type = "test_program_type",
				Description = "test_program_description"
			};

			user.CreatedPrograms.Add(defaultProgram);

			this.dbContext.SaveChanges();

			return defaultProgram;
		}
	}
}
