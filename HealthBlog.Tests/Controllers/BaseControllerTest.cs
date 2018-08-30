using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace HealthBlog.Tests.Controllers
{
	[TestClass]
	public abstract class BaseControllerTestsClass
	{
		protected ControllerContext controllerContext;

		[TestInitialize]
		public void InitializeTest()
		{
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, "test"),
			}));

			controllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext() { User = user }
			};
		}
	}
}
