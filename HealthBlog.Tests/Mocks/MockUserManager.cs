using HealthBlog.Data;
using HealthBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace HealthBlog.Tests.Mocks
{
	public static class MockUserManager
	{
		public const string testUsername = "testUsername";
		public const string testUserId = "testId";

		public static UserManager<User> GetUserManager(HealthBlogDbContext dbContext)
		{
			var mockUserManager = new Mock<UserManager<User>>(
					new Mock<IUserStore<User>>().Object,
					new Mock<IOptions<IdentityOptions>>().Object,
					new Mock<IPasswordHasher<User>>().Object,
					new IUserValidator<User>[0],
					new IPasswordValidator<User>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<IServiceProvider>().Object,
					new Mock<ILogger<UserManager<User>>>().Object);

			var testUser = new User()
			{
				Id = testUserId,
				UserName = testUsername
			};

			dbContext.Users.Add(testUser);
			dbContext.SaveChanges();

			mockUserManager.Setup(exp => exp.FindByNameAsync(testUsername))
				.ReturnsAsync(testUser);

			return mockUserManager.Object;
		}
	}
}
