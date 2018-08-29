namespace HealthBlog.Common.Users.ViewModels
{
	using System.Collections.Generic;

	using BindingModels;

	public class AddMealToDayModel
    {
		public int Id { get; set; }

		public IEnumerable<AddMealToDayBindingModel> Meals { get; set; }
	}
}
