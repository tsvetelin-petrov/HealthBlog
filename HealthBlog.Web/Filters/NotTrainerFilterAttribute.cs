namespace HealthBlog.Web.Filters
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;

	using System;
	using HealthBlog.Common.Constants;

	public class NotTrainerFilterAttribute : Attribute, IPageFilter
	{
		public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
		{
		}

		public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			if (context.HttpContext.User.IsInRole(RolesConstants.Trainer))
			{
				context.Result = new RedirectResult(UrlConstants.Home);
				return;
			}
		}

		public void OnPageHandlerSelected(PageHandlerSelectedContext context)
		{
		}
	}
}
