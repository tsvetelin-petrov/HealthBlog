using HealthBlog.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HealthBlog.Tests.Mocks
{
	public static class MockDbContext
    {
		public static HealthBlogDbContext GetContext()
		{
			var options = new DbContextOptionsBuilder<HealthBlogDbContext>()
			   .UseInMemoryDatabase(Guid.NewGuid().ToString())
			   .Options;

			return new HealthBlogDbContext(options);
		}
	}
}
