namespace HealthBlog.Models
{
	using HealthBlog.Common.Constants;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Meal
	{
		public Meal()
		{
			this.Days = new List<MealDay>();
		}

		public int Id { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.NameMaxLength, MinimumLength = ModelsLengthConstants.NameMinLength)]
		public string Name { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.DescriptionMaxLength, MinimumLength = ModelsLengthConstants.DescriptionMinLength)]
		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<MealDay> Days { get; set; }
	}
}