namespace HealthBlog.Common.Users.ViewModels
{
	using System.Collections.Generic;

	using BindingModels;

	public class AddTrainingToDayModel
    {
		public int Id { get; set; }

		public IEnumerable<AddTrainingToDayBindingModel> Trainings { get; set; }
	}
}
