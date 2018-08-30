namespace HealthBlog.Common.Users.BindingModels
{
	using Microsoft.AspNetCore.Mvc.Rendering;
	using System.Collections.Generic;

	public class AddTrainingToDayModel
	{
		public int Id { get; set; }

		public IEnumerable<SelectListItem> Trainings { get; set; }
	}
}
