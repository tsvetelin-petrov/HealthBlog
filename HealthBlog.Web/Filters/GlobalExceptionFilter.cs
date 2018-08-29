namespace HealthBlog.Web.Filters
{
	using HealthBlog.Common.Exceptions;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;

	public class GlobalExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			if (context.Exception is HealthBlogBaseException)
			{
				context.Result = new RedirectResult($"/Home/Error?message={context.Exception.Message}");
			}
			else
			{
				context.Result = new RedirectResult($"/Home/Error");
			}
		}
	}
}
