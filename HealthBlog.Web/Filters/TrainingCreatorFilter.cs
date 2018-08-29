namespace HealthBlog.Web.Filters
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using System.Threading.Tasks;

	using HealthBlog.Services.Users.Contracts;
	using HealthBlog.Common.Constants;

	public class TrainingCreatorFilter : IAsyncActionFilter
	{
		private readonly ITrainingsService trainingsService;

		public TrainingCreatorFilter(ITrainingsService trainingsService)
		{
			this.trainingsService = trainingsService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var username = context.HttpContext.User.Identity.Name;
			var parameters = context.ActionArguments;
			if (parameters.TryGetValue("id", out var idObj))
			{
				int id = (int)idObj;
				var isCreator = await this.trainingsService.IsCreatorUserAsync(username, id);

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
