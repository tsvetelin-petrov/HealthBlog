using HealthBlog.Common.Constants;
using HealthBlog.Web.Areas.Trainer.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HealthBlog.Tests.Controllers.Trainers
{
	[TestClass]
	public class TrainerAccessTests
	{
		[TestMethod]
		public void ProgramsController_ShouldBeInTrainerArea()
		{
			var area = typeof(ProgramsController)
				.GetCustomAttributes(true)
				.FirstOrDefault(attr => attr is AreaAttribute) as AreaAttribute;

			Assert.IsNotNull(area);
			Assert.AreEqual(RolesConstants.Trainer, area.RouteValue);
		}

		[TestMethod]
		public void ProgramsController_ShouldAuthorizeTrainers()
		{
			var authorization = typeof(ProgramsController)
				.GetCustomAttributes(true)
				.FirstOrDefault(attr => attr is AuthorizeAttribute) as AuthorizeAttribute;

			Assert.IsNotNull(authorization);
			Assert.AreEqual(RolesConstants.Trainer, authorization.Roles);
		}
	}
}
