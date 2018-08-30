using HealthBlog.Common.Constants;
using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Services.Users.Contracts;
using HealthBlog.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace HealthBlog.Tests.Controllers.Users
{
	[TestClass]
	public class TrainingsControllerCreateTests : BaseControllerTestsClass
	{
		private const string name = "name";
		private const string type = "type";
		private const string description = "description";
		private const string empty = "1";

		[TestMethod]
		public async Task Create_WithValidModel_ShouldRedirectToDetailsAction()
		{
			var model = new TrainingCreateBindingModel()
			{
				Name = name,
				Type = type,
				Description = description
			};

			var result = await this.GetTrainingsController().Create(model);

			Assert.IsTrue(result is RedirectToActionResult);
			Assert.AreEqual(ActionConstants.Details, ((RedirectToActionResult)result).ActionName);
		}

		[TestMethod]
		public async Task Create_WithUnvalidMethod_ShouldReturnSaveView()
		{
			var controller = this.GetTrainingsController();

			controller.ModelState.AddModelError("", "Invalid Data");

			var result = await controller.Create(null);

			Assert.IsTrue(result is ViewResult);
		}

		private TrainingsController GetTrainingsController()
		{
			var mockDaysService = new Mock<ITrainingsService>();
			mockDaysService
				.Setup(opt => opt.CreateTrainingAsync(It.IsAny<TrainingCreateBindingModel>(), It.IsAny<string>()))
				.ReturnsAsync(() => 0);

			var controller = new TrainingsController(
				mockDaysService.Object)
			{
				ControllerContext = this.controllerContext
			};

			return controller;
		}
	}
}
