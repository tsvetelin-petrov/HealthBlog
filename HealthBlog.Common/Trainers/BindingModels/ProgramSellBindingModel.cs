namespace HealthBlog.Common.Trainers.BindingModels
{
	using System;
	using System.ComponentModel.DataAnnotations;

	using Constants;
	
	public class ProgramSellBindingModel
    {
		private const string priceErrorMessage = "Цената трябва да е положително число.";

		public int Id { get; set; }

		[Required]
		[Range(0, Double.PositiveInfinity, ErrorMessage = priceErrorMessage)]
		[Display(Name = AttributeDisplayNameConstants.Price)]
		public decimal Price { get; set; }

		public string Name { get; set; }
	}
}
