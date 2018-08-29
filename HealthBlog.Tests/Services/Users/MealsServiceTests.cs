namespace HealthBlog.Tests.Services.Users
{
	using HealthBlog.Common.Exceptions;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using HealthBlog.Services.Users;
	using HealthBlog.Tests.Mocks;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	[TestClass]
	public class MealsServiceTests
	{
		private const string meal1Name = "Meal1";
		private const string meal2Name = "Meal2";
		private const string meal3Name = "Meal3";
		private HealthBlogDbContext dbContext;
		private MealsService service;

		[TestMethod]
		public async Task CreateMealAsync_WithValidModel_ShouldAddMealInDatabase()
		{
			MealCreateBindingModel model = new MealCreateBindingModel();

			await service.CreateMealAsync(model, MockUserManager.testUsername);
			
			Assert.AreEqual(1, dbContext.Meals.Count());
		}

		[TestMethod]
		public async Task CreateMealAsync_WithNull_ShouldThrowException()
		{
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => service.CreateMealAsync(null, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task GetAllMealDetailssAsync_WithSomeMealsInDatabase_ShouldReturnTheirViewModels()
		{
			Meal meal1 = new Meal()
			{
				Name = meal1Name
			};
			Meal meal2 = new Meal()
			{
				Name = meal2Name
			};
			Meal meal3 = new Meal()
			{
				Name = meal3Name
			};

			var user = await this.dbContext.Users
				.FirstOrDefaultAsync(u => u.Id == MockUserManager.testUserId);

			user.Meals.Add(meal1);
			user.Meals.Add(meal2);
			user.Meals.Add(meal3);

			await this.dbContext.SaveChangesAsync();

			var meals = await service.GetAllMealDetailssAsync(MockUserManager.testUsername);

			Assert.AreEqual(3, meals.Count());
			Assert.IsTrue(meals.All(m => m != null));
		}

		[TestMethod]
		public async Task GetMealAsync_TryGetOwnedMeal_ShouldReturnIt()
		{

			Meal meal1 = new Meal()
			{
				Name = meal1Name
			};

			var user = await this.dbContext.Users
				.FirstOrDefaultAsync(u => u.Id == MockUserManager.testUserId);

			user.Meals.Add(meal1);

			await this.dbContext.SaveChangesAsync();

			var mealFromService = await this.service.GetMealAsync(meal1.Id, MockUserManager.testUsername);

			Assert.AreSame(meal1, mealFromService);
		}

		[TestMethod]
		public async Task GetMealAsync_TryGetUnownedMeal_ShouldThrowException()
		{
			Meal meal1 = new Meal()
			{
				Name = meal1Name
			};

			var user = new User()
			{
				UserName = "testUsername2"
			};

			this.dbContext.Users.Add(user);

			user.Meals.Add(meal1);

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<ValidationMealException>(() => this.service.GetMealAsync(meal1.Id, MockUserManager.testUsername));
		}

		[TestInitialize]
		public virtual void InitializeTests()
		{
			this.dbContext = MockDbContext.GetContext();
			var userManager = MockUserManager.GetUserManager(dbContext);
			this.service = new MealsService(
				dbContext,
				MockAutoMapper.GetAutoMapper(),
				userManager);
		}
	}
}
