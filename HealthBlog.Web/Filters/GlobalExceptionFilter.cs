namespace HealthBlog.Web.Filters
{
	using HealthBlog.Common.Exceptions;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;

	public class GlobalExceptionFilter : IExceptionFilter
	{
		private readonly ITempDataDictionaryFactory tempDataFactory;

		public GlobalExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
		{
			this.tempDataFactory = tempDataFactory;
		}

		public void OnException(ExceptionContext context)
		{
			var tempData = tempDataFactory.GetTempData(context.HttpContext);

			if (context.Exception is HealthBlogBaseException)
			{
				tempData.Add("Message", context.Exception.Message);
			}

			context.Result = new RedirectResult("/Home/Error");
		}
	}
}
