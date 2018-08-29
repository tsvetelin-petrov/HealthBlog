using HealthBlog.Common.Exceptions;
using HealthBlog.Data;
using HealthBlog.Models;
using HealthBlog.Services.Trainers;
using HealthBlog.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBlog.Tests.Services.Trainers
{
	[TestClass]
	public class TrainerProgramServiceTests
	{
		private const string programName = "program";
		private const string programType = "type";
		private const string programDescription = "description";
		private const string userUsername = "username";

		private HealthBlogDbContext dbContext;
		private TrainersProgramsService service;

		[TestMethod]
		public async Task DeleteDayAsync_WithValidParameters_ShouldRemoveDayFromProgram()
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

			var day1 = new Day()
			{
				Author = user
			};

			var day2 = new Day()
			{
				Author = user
			};

			program.Days.Add(new ProgramDay()
			{
				Day = day1
			});

			program.Days.Add(new ProgramDay()
			{
				Day = day2
			});

			await this.dbContext.SaveChangesAsync();

			await this.service.DeleteDayAsync(day1.Id, program.Id, user.UserName);

			var programFromDb = await this.dbContext.Programs.FindAsync(program.Id);

			Assert.AreEqual(1, programFromDb.Days.Count);
			Assert.AreEqual(day2.Id, programFromDb.Days.First().DayId);
		}

		[TestMethod]
		public async Task DeleteDayAsync_NotOwnedProgram_ShouldThrowException()
		{
			var user = new User()
			{
				UserName = userUsername
			};

			var program = new Program()
			{
				Name = programName,
				Type = programType,
				Description = programDescription,
				Author = user
			};

			user.CreatedPrograms.Add(program);

			var day1 = new Day()
			{
				Author = user
			};

			var day2 = new Day()
			{
				Author = user
			};

			program.Days.Add(new ProgramDay()
			{
				Day = day1
			});

			program.Days.Add(new ProgramDay()
			{
				Day = day2
			});

			await this.dbContext.SaveChangesAsync();

			await Assert.ThrowsExceptionAsync<InvalidProgramException>(() => this.service.DeleteDayAsync(day1.Id, program.Id, MockUserManager.testUsername));

		}

		[TestMethod]
		public async Task GetAllProgramsForAdding_WithFewPrograms_ShouldReturnCreated()
		{
			var user1 = await this.dbContext.Users.FindAsync(MockUserManager.testUserId);

			user1.CreatedPrograms.Add(new Program()
			{
				Name = programName + "1",
				Type = programType + "1",
				Description = programDescription + "1",
				Author = user1
			});

			user1.CreatedPrograms.Add(new Program()
			{
				Name = programName + "2",
				Type = programType + "2",
				Description = programDescription + "2",
				Author = user1
			});

			var user2 = new User()
			{
				UserName = userUsername
			};

			var program3 = new Program()
			{
				Name = programName + "3",
				Type = programType + "3",
				Description = programDescription + "3",
				Author = user1
			};

			user2.CreatedPrograms.Add(program3);

			user1.OwnedPrograms.Add(new UserProgram()
			{
				Program = program3
			});

			this.dbContext.Users.Add(user2);
			await this.dbContext.SaveChangesAsync();

			var programs = await this.service.GetAllProgramsForAdding(user1.UserName);

			Assert.AreEqual(2, programs.Count());
		}

		[TestMethod]
		public async Task GetProgramByIdAsync_TryGetCreatedProgram_ShouldReturnIt()
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

			var programFromService = await this.service.GetProgramByIdAsync(program.Id, user.UserName);

			Assert.IsTrue(programFromService != null);
			Assert.AreSame(programFromService.Name, program.Name);
			Assert.AreEqual(programFromService.Id, program.Id);
		}

		[TestMethod]
		public async Task GetProgramByIdAsync_TryGetBoughtProgram_ShouldThrowException()
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

			await Assert.ThrowsExceptionAsync<InvalidProgramException>(() => this.service.GetProgramByIdAsync(program.Id, buyer.UserName));
		}

		[TestInitialize]
		public virtual void InitializeTests()
		{
			this.dbContext = MockDbContext.GetContext();
			var userManager = MockUserManager.GetUserManager(dbContext);
			this.service = new TrainersProgramsService(
				dbContext,
				MockAutoMapper.GetAutoMapper(),
				userManager);
		}
	}
}
