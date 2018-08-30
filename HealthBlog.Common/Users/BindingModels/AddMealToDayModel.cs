namespace HealthBlog.Common.Users.BindingModels
{
	using Microsoft.AspNetCore.Mvc.Rendering;
	using System.Collections.Generic;

	public class AddMealToDayModel
    {
		public int Id { get; set; }

		public IEnumerable<SelectListItem> Meals { get; set; }
	}
}
