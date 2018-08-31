using HealthBlog.Common.Exceptions;
using HealthBlog.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace HealthBlog.Tests.Filters
{
	[TestClass]
	public class OnExceptionTests
	{
		[TestMethod]
		public void OnException_WhitCustomExcepton_ShouldReturnRedirectResultAndAddMessageToTempData()
		{
			var context = new ExceptionContext(
							new ActionContext(
								new DefaultHttpContext(),
								new RouteData(),
								new ActionDescriptor()),
							new List<IFilterMetadata>())
			{
				Exception = new CertificateUploadTimesException()
			};

			var mockTempDataFactory = new Mock<ITempDataDictionaryFactory>();
			var mockTempData = new Mock<ITempDataDictionary>();
			bool isCalled = false;
			mockTempData
				.Setup(opt => opt.Add(It.IsAny<string>(), It.IsAny<object>()))
				.Callback(() => isCalled = true);
			mockTempDataFactory
				.Setup(opt => opt.GetTempData(It.IsAny<HttpContext>()))
				.Returns(mockTempData.Object);
			var exceptionFilter = new GlobalExceptionFilter(mockTempDataFactory.Object);

			exceptionFilter.OnException(context);

			Assert.IsTrue(context.Result is RedirectResult);
			Assert.IsTrue(isCalled);
		}

		[TestMethod]
		public void OnException_WhitCustomExcepton_ShouldReturnRedirectResultWithoutMessageInTempData()
		{

			var context = new ExceptionContext(
							new ActionContext(
								new DefaultHttpContext(),
								new RouteData(),
								new ActionDescriptor()),
							new List<IFilterMetadata>())
			{
				Exception = new Exception()
			};

			var mockTempDataFactory = new Mock<ITempDataDictionaryFactory>();
			var mockTempData = new Mock<ITempDataDictionary>();
			bool isCalled = false;
			mockTempData
				.Setup(opt => opt.Add(It.IsAny<string>(), It.IsAny<object>()))
				.Callback(() => isCalled = true);
			mockTempDataFactory
				.Setup(opt => opt.GetTempData(It.IsAny<HttpContext>()))
				.Returns(mockTempData.Object);
			var exceptionFilter = new GlobalExceptionFilter(mockTempDataFactory.Object);

			exceptionFilter.OnException(context);

			Assert.IsTrue(context.Result is RedirectResult);
			Assert.IsFalse(isCalled);
		}
	}
}
