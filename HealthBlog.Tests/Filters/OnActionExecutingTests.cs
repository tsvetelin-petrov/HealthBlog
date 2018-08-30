namespace HealthBlog.Tests.Filters
{
	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Web.Filters;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Abstractions;
	using Microsoft.AspNetCore.Mvc.Filters;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;

	[TestClass]
	public class OnActionExecutingTests
	{
		[TestMethod]
		public async Task OnActionExecutingAsync_WithValidUserAndId_ShouldReturnNothing()
		{
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, "test"),
			}));

			var mockContext = new Mock<ActionExecutingContext>(
							new ActionContext(new DefaultHttpContext()
							{
								User = user
							}, new RouteData(), new ActionDescriptor()),
							new List<IFilterMetadata>(),
							new Dictionary<string, object>(),
							new Mock<Controller>());

			var actionDelegate = new Mock<ActionExecutionDelegate>();
			object value = 1;
			mockContext
				.Setup(opt => opt.ActionArguments.TryGetValue(It.IsAny<string>(), out value))
				.Returns(true);

			var daysService = new Mock<IDaysService>();
			daysService
				.Setup(opt => opt.IsCreatorUserAsync(It.IsAny<string>(), It.IsAny<int>()))
				.ReturnsAsync(true);

			var filter = new DayCreatorFilter(daysService.Object);
			await filter.OnActionExecutionAsync(mockContext.Object, actionDelegate.Object);

			Assert.IsNull(mockContext.Object.Result);
		}

		[TestMethod]
		public async Task OnActionExecutingAsync_WithInvalidUserAndId_ShouldReturnRedirectResult()
		{
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, "test"),
			}));

			var context = new ActionExecutingContext(
							new ActionContext(new DefaultHttpContext()
							{
								User = user
							}, new RouteData(), new ActionDescriptor()),
							new List<IFilterMetadata>(),
							new Dictionary<string, object>(),
							new Mock<Controller>());

			var actionDelegate = new Mock<ActionExecutionDelegate>();
			object value = 1;
			context.ActionArguments.Add("id", value);


			var daysService = new Mock<IDaysService>();
			daysService
				.Setup(opt => opt.IsCreatorUserAsync(It.IsAny<string>(), It.IsAny<int>()))
				.ReturnsAsync(false);

			var filter = new DayCreatorFilter(daysService.Object);
			await filter.OnActionExecutionAsync(context, actionDelegate.Object);

			Assert.IsInstanceOfType(context.Result, typeof(RedirectResult));
		}
	}
}
