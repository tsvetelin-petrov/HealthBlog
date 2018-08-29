using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Data;
using HealthBlog.Models;
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
	public class TrainingsServiceTests
	{
		private const string trainingName = "training";
		private const string userUsername = "testUsername2";
		private const string exerciseName = "exercise";
		private const string targertMuscleConstant = "muscle";
		private const string programName = "programName";
		private const string programType = "programType";
		private const string programDescription = "programDescription";

		private HealthBlogDbContext dbContext;
		private TrainingsService service;

		[TestMethod]
		public async Task CreateTrainingAsync_WithValidModel_ShouldCreateIt()
		{
			var model = new TrainingCreateBindingModel()
			{
				Name = trainingName
			};

			await this.service.CreateTrainingAsync(model, MockUserManager.testUsername);

			Assert.AreEqual(1, this.dbContext.Trainings.Count());
			Assert.AreEqual(MockUserManager.testUserId, this.dbContext.Trainings.First().UserId);
		}

		[TestMethod]
		public async Task CreateTrainingAsync_WithoutModel_ShouldThrowException()
		{
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => this.service.CreateTrainingAsync(null, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task IsValidTrainingAsync_ValidateOwnedTraining_ShouldReturnTrue()
		{
			var training = new Training()
			{
				Name = trainingName
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			user.Trainings.Add(training);

			await this.dbContext.SaveChangesAsync();

			Assert.IsTrue(await this.service.IsValidTrainingAsync(training.Id, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task IsValidTrainingAsync_ValidateUnownedTraining_ShouldReturnFalse()
		{
			var training = new Training()
			{
				Name = trainingName
			};

			var user = new User()
			{
				UserName = userUsername
			};

			user.Trainings.Add(training);

			await this.dbContext.SaveChangesAsync();

			Assert.IsFalse(await this.service.IsValidTrainingAsync(training.Id, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task IsValidTrainingAsync_ValidateBoughtProgramTraining_ShouldReturnTrue()
		{
			var creator = new User()
			{
				UserName = userUsername
			};

			var program = new Program()
			{
				Name = programName,
				Type = programType,
				Description = programDescription,
				IsForSale = true,
				Price = 0m
			};

			var training = new Training()
			{
				Name = trainingName
			};

			var day = new Day()
			{
				Author = creator
			};
			day.Trainings.Add(new TrainingDay()
			{
				Training = training
			});
			program.Days.Add(new ProgramDay()
			{
				Day = day
			});
			creator.CreatedPrograms.Add(program);

			var buyer = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			buyer.OwnedPrograms.Add(new UserProgram()
			{
				Program = program,
			});

			await this.dbContext.SaveChangesAsync();

			Assert.IsTrue(await this.service.IsValidTrainingAsync(training.Id, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task AddExerciseToTrainingAsync_WithValidModel_ShouldAddTheExerciseToTheTraining()
		{
			var training = new Training()
			{
				Name = trainingName
			};

			var exercise = new Exercise()
			{
				Name = exerciseName,
				TargetMuscle = targertMuscleConstant
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			user.Trainings.Add(training);
			user.Exercises.Add(exercise);

			await this.dbContext.SaveChangesAsync();

			TrainingExerciseInput model = new TrainingExerciseInput()
			{
				ExerciseId = exercise.Id,
				RepetitionCount = 1,
				SeriesCount = 1
			};

			await this.service.AddExerciseToTrainingAsync(model, training.Id, MockUserManager.testUsername);

			Assert.AreEqual(1, user.Trainings.First().Exercises.Count);
			Assert.AreSame(exerciseName, user.Trainings.First().Exercises.First().Exercise.Name);
		}

		[TestMethod]
		public async Task AddExerciseToTrainingAsync_WithoutModel_ShouldThrowException()
		{
			var training = new Training()
			{
				Name = trainingName
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			user.Trainings.Add(training);

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => this.service.AddExerciseToTrainingAsync(null, training.Id, MockUserManager.testUsername));
		}


		[TestInitialize]
		public virtual void InitializeTests()
		{
			this.dbContext = MockDbContext.GetContext();
			var userManager = MockUserManager.GetUserManager(dbContext);
			var exercisesService = new Mock<IExercisesService>();

			this.service = new TrainingsService(
				dbContext,
				MockAutoMapper.GetAutoMapper(),
				userManager,
				exercisesService.Object);
		}
	}
}
