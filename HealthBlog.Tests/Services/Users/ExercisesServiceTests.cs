using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Data;
using HealthBlog.Services.Users;
using HealthBlog.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using HealthBlog.Models;
using HealthBlog.Common.Exceptions;

namespace HealthBlog.Tests.Services.Users
{
	[TestClass]
	public class ExercisesServiceTests
	{
		private const string exerciseName = "exercise";
		private const string targertMuscleConstant = "muscle";

		private HealthBlogDbContext dbContext;
		private ExercisesService service;

		[TestMethod]
		public async Task CreateExerciseAsync_WithValidModel_ShouldCreateExercise()
		{
			var model = new ExerciseCreateBindingModel()
			{
				Name = exerciseName
			};

			await this.service.CreateExerciseAsync(model, MockUserManager.testUsername);

			Assert.AreEqual(1, this.dbContext.Exercises.Count());
		}

		[TestMethod]
		public async Task CreateExerciseAsync_WithoutModel_ShoulThrowException()
		{
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => this.service.CreateExerciseAsync(null, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task GetExerciseAsync_TryGetValidExercise_ShouldReturnIt()
		{
			var exercise = new Exercise()
			{
				Name = exerciseName,
				TargetMuscle = targertMuscleConstant
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			user.Exercises.Add(exercise);

			await this.dbContext.SaveChangesAsync();

			var exerciseFromService = await this.service.GetExerciseAsync(exercise.Id, user.Id);

			Assert.AreSame(exercise, exerciseFromService);
		}

		[TestMethod]
		public async Task GetExerciseAsync_TryGetUnownedExercise_ShouldThrowException()
		{
			var exercise = new Exercise()
			{
				Name = exerciseName,
				TargetMuscle = targertMuscleConstant
			};

			var user = new User()
			{
				UserName = "testUser2"
			};

			this.dbContext.Users.Add(user);

			user.Exercises.Add(exercise);

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<InvalidExerciseException>(() => this.service.GetExerciseAsync(exercise.Id, MockUserManager.testUserId));
		}

		[TestMethod]
		public async Task DeleteExerciseAsync_TryDeleteOwnedExercise_ShouldDeleteIt()
		{
			var exercise = new Exercise()
			{
				Name = exerciseName,
				TargetMuscle = targertMuscleConstant
			};

			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			user.Exercises.Add(exercise);

			await this.dbContext.SaveChangesAsync();

			await this.service.DeleteExerciseAsync(exercise.Id, MockUserManager.testUsername);

			Assert.AreEqual(0, this.dbContext.Exercises.Count());
		}

		[TestMethod]
		public async Task DeleteExerciseAsync_TryDeleteUnownedExercise_ShouldThrowException()
		{
			var exercise = new Exercise()
			{
				Name = exerciseName,
				TargetMuscle = targertMuscleConstant
			};

			var user = new User()
			{
				UserName = "testUser2"
			};

			this.dbContext.Users.Add(user);

			user.Exercises.Add(exercise);

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<InvalidExerciseException>(() => this.service.DeleteExerciseAsync(exercise.Id, MockUserManager.testUsername));
		}

		[TestInitialize]
		public virtual void InitializeTests()
		{
			this.dbContext = MockDbContext.GetContext();
			var userManager = MockUserManager.GetUserManager(dbContext);
			this.service = new ExercisesService(
				dbContext,
				MockAutoMapper.GetAutoMapper(),
				userManager);
		}
	}
}
