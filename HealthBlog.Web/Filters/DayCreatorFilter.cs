namespace HealthBlog.Web.Filters
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using System.Threading.Tasks;

	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Common.Constants;

	public class DayCreatorFilter : ActionFilterAttribute
	{
		private readonly IDaysService daysService;

		public DayCreatorFilter(IDaysService daysService)
		{
			this.daysService = daysService;
		}

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var username = context.HttpContext.User.Identity.Name;
			var parameters = context.ActionArguments;
			if (parameters.TryGetValue("id", out var idObj))
			{
				int id = (int)idObj;
				var isCreator = await this.daysService.IsCreatorUserAsync(username, id);

				if (!isCreator)
				{
					context.Result = new RedirectResult(UrlConstants.Home);
					return;
				}

				await next();
			}
		}
	}
}
