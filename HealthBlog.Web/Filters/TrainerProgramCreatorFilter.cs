namespace HealthBlog.Web.Filters
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using System.Threading.Tasks;

	using Common;
	using HealthBlog.Services.Trainers.Contracts;
	using HealthBlog.Common.Constants;

	public class TrainerProgramCreatorFilter : ActionFilterAttribute
	{
		private readonly ITrainersProgramsService programsService;

		public TrainerProgramCreatorFilter(ITrainersProgramsService programsService)
		{
			this.programsService = programsService;
		}

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var username = context.HttpContext.User.Identity.Name;
			var parameters = context.ActionArguments;

			if (parameters.TryGetValue("id", out var idObj) || parameters.TryGetValue("programId", out idObj))
			{
				int id = (int)idObj;
				var isCreator = await this.programsService.IsCreatorUserAsync(username, id);

				if (!isCreator)
				{
					context.Result = new RedirectResult(UrlConstants.Home);
					return;
				}
			}

			await next();
		}
	}
}
