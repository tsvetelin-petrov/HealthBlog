using HealthBlog.Common.Constants;
using HealthBlog.Common.Users.ViewModels;
using HealthBlog.Services.Trainers.Contracts;
using HealthBlog.Services.Users.Contracts;
using HealthBlog.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthBlog.Tests.Controllers.Users
{
	[TestClass]
	public class DaysControllerTests : BaseControllerTest
	{
		[TestMethod]
		public async Task Create_WithoutParameters_ShouldCallService()
		{
			bool isCalledService = false;

			var mockDaysService = new Mock<IDaysService>();
			mockDaysService
				.Setup(opt => opt.CreateDayAsync(It.IsAny<string>()))
				.Callback(() => isCalledService = true)
				.ReturnsAsync(() => 0);
			var mockTrainngsService = new Mock<ITrainingsService>();
			var mockTrainerProgramsService = new Mock<ITrainersProgramsService>();

			var controller = new DaysController(
				mockDaysService.Object,
				mockTrainngsService.Object,
				mockTrainerProgramsService.Object)
			{
				ControllerContext = this.controllerContext
			};

			var result = await controller.Create();

			Assert.IsTrue(isCalledService);
		}

		[TestMethod]
		public async Task Create_WithoutParameters_ShouldReturnRedirectToActionResult()
		{
			var mockDaysService = new Mock<IDaysService>();
			mockDaysService
				.Setup(opt => opt.CreateDayAsync(It.IsAny<string>()))
				.ReturnsAsync(() => 0);
			var mockTrainngsService = new Mock<ITrainingsService>();
			var mockTrainerProgramsService = new Mock<ITrainersProgramsService>();

			var controller = new DaysController(
				mockDaysService.Object,
				mockTrainngsService.Object,
				mockTrainerProgramsService.Object)
			{
				ControllerContext = this.controllerContext
			};

			var result = await controller.Create();

			Assert.IsTrue(result is RedirectToActionResult);
		}

		[TestMethod]
		public async Task Create_WithoutParameters_ShouldRedirectToCorrectAction()
		{
			var mockDaysService = new Mock<IDaysService>();
			mockDaysService
				.Setup(opt => opt.CreateDayAsync(It.IsAny<string>()))
				.ReturnsAsync(() => 0);
			var mockTrainngsService = new Mock<ITrainingsService>();
			var mockTrainerProgramsService = new Mock<ITrainersProgramsService>();

			var controller = new DaysController(
				mockDaysService.Object,
				mockTrainngsService.Object,
				mockTrainerProgramsService.Object)
			{
				ControllerContext = this.controllerContext
			};

			var result = await controller.Create();

			Assert.AreEqual(ActionConstants.Details, ((RedirectToActionResult)result).ActionName);
			Assert.AreEqual(ControllerConstants.Days, ((RedirectToActionResult)result).ControllerName);
		}
	}
}
