using HealthBlog.Common.Exceptions;
using HealthBlog.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HealthBlog.Tests.Filters
{
	[TestClass]
	public class OnExceptionTests
	{
		[TestMethod]
		public void OnException_WhitCustomExcepton_ShouldReturnRedirectResultWithMessage()
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

			var exceptionFilter = new GlobalExceptionFilter();

			exceptionFilter.OnException(context);

			Assert.IsTrue(context.Result is RedirectResult);
			Assert.IsTrue(((RedirectResult)context.Result).Url.EndsWith(context.Exception.Message));
		}

		[TestMethod]
		public void OnException_WhitCustomExcepton_ShouldReturnRedirectResultWithoutMessage()
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

			var exceptionFilter = new GlobalExceptionFilter();

			exceptionFilter.OnException(context);

			Assert.IsTrue(context.Result is RedirectResult);
			Assert.IsFalse(((RedirectResult)context.Result).Url.EndsWith(context.Exception.Message));
		}
	}
}
