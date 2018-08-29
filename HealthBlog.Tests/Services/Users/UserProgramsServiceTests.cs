using HealthBlog.Common.Exceptions;
using HealthBlog.Data;
using HealthBlog.Models;
using HealthBlog.Services.Users;
using HealthBlog.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBlog.Tests.Services.Users
{
	[TestClass]
	public class UserProgramsServiceTests
	{
		private const string searchTerm = "2";
		private const string programName = "program";
		private const string programType = "type";
		private const string programDescription = "description";
		private const string userUsername = "username";

		private HealthBlogDbContext dbContext;
		private UserProgramsService service;

		[TestMethod]
		public async Task GetProgramsForBuyingAsync_WithoutSearchTerm_ShouldReturnAll()
		{
			var user1 = new User()
			{
				UserName = "user1"
			};
			user1.CreatedPrograms.Add(new Program()
			{
				Description = "",
				IsForSale = true,
				Price = 0,
				Name = "user1program",
				Type = "type"
			});
			var user2 = new User()
			{
				UserName = "user2"
			};
			user1.CreatedPrograms.Add(new Program()
			{
				Description = "",
				IsForSale = true,
				Price = 0,
				Name = "user2program",
				Type = "type"
			});

			await this.dbContext.AddRangeAsync(new User[] { user1, user2 });

			await this.dbContext.SaveChangesAsync();

			var programs = await this.service.GetProgramsForBuyingAsync(MockUserManager.testUsername, null);

			Assert.AreEqual(2, programs.Count());
		}

		[TestMethod]
		public async Task GetProgramsForBuyingAsync_WithSearchTerm_ShouldReturnSomeOfThem()
		{
			var user1 = new User()
			{
				UserName = "user1"
			};
			user1.CreatedPrograms.Add(new Program()
			{
				Description = "",
				IsForSale = true,
				Price = 0,
				Name = "user1program",
				Type = "type"
			});
			var user2 = new User()
			{
				UserName = "user2"
			};
			user1.CreatedPrograms.Add(new Program()
			{
				Description = "",
				IsForSale = true,
				Price = 0,
				Name = "user2program",
				Type = "type"
			});

			await this.dbContext.AddRangeAsync(new User[] { user1, user2 });

			await this.dbContext.SaveChangesAsync();

			var programs = await this.service.GetProgramsForBuyingAsync(MockUserManager.testUsername, searchTerm);

			Assert.AreEqual(1, programs.Count());
		}

		[TestMethod]
		public async Task BuyProgramAsync_TryBuyValidProgram_ShouldAddItToOwnedPrograms()
		{
			var programCreator = new User()
			{
				UserName = userUsername
			};

			var program = new Program()
			{
				Name = programName,
				Type = programType,
				Description = programDescription
			};

			programCreator.CreatedPrograms.Add(program);

			this.dbContext.Users.Add(programCreator);

			await this.dbContext.SaveChangesAsync();

			await this.service.BuyProgramAsync(program.Id, MockUserManager.testUsername);

			var programFromDb = (await this.dbContext.Users.FindAsync(MockUserManager.testUserId)).OwnedPrograms.First().Program;

			Assert.IsTrue(programFromDb != null);
			Assert.AreSame(program.Name, programFromDb.Name);
		}

		[TestMethod]
		public async Task BuyProgramAsync_TryBuyYourProgram_ShouldThrowException()
		{
			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			var program = new Program()
			{
				Name = programName,
				Type = programType,
				Description = programDescription,
				Author = user
			};

			user.CreatedPrograms.Add(program);

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<InvalidProgramException>(() => this.service.BuyProgramAsync(program.Id, MockUserManager.testUsername));
		}

		[TestMethod]
		public async Task GetProgramDetailsAsync_TryGetCreatedProgram_ShouldReturnIt()
		{
			var user = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			var program = new Program()
			{
				Name = programName,
				Type = programType,
				Description = programDescription
			};

			user.CreatedPrograms.Add(program);

			await this.dbContext.SaveChangesAsync();

			var programFromService = await this.service.GetProgramDetailsAsync(user.UserName, program.Id);

			Assert.IsTrue(programFromService != null);
			Assert.AreSame(program.Name, programFromService.Name);
			Assert.IsTrue(programFromService.IsCreatedByCurrentUser);
		}

		[TestMethod]
		public async Task GetProgramDetailsAsync_TryGetBoughtProgram_ShouldReturnIt()
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
				Author = creator
			};

			creator.CreatedPrograms.Add(program);

			var buyer = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			buyer.OwnedPrograms.Add(new UserProgram()
			{
				Program = program
			});

			await this.dbContext.SaveChangesAsync();

			var programFromService = await this.service.GetProgramDetailsAsync(MockUserManager.testUsername, program.Id);

			Assert.IsTrue(programFromService != null);
			Assert.AreSame(program.Name, programFromService.Name);
			Assert.IsFalse(programFromService.IsCreatedByCurrentUser);
		}

		[TestMethod]
		public async Task GetProgramDetailsAsync_TryGetUnvalidProgram_ShouldThrowException()
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
				Author = creator
			};

			creator.CreatedPrograms.Add(program);

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<InvalidProgramException>(() => this.service.GetProgramDetailsAsync(MockUserManager.testUsername, program.Id));
		}

		[TestInitialize]
		public virtual void InitializeTests()
		{
			this.dbContext = MockDbContext.GetContext();
			var userManager = MockUserManager.GetUserManager(dbContext);
			this.service = new UserProgramsService(
				dbContext,
				MockAutoMapper.GetAutoMapper(),
				userManager);
		}
	}
}
