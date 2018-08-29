namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	public class ProgramCreateBindingModel
    {
		private const string nameDisplay = "Име";
		private const string typeDisplay = "Тип";
		private const string descriptionDisplay = "Описание";

		[Required]
		[Display(Name = nameDisplay)]
		public string Name { get; set; }

		[Required]
		[Display(Name = typeDisplay)]
		public string Type { get; set; }

		[Required]
		[Display(Name = descriptionDisplay)]
		public string Description { get; set; }
	}
}
