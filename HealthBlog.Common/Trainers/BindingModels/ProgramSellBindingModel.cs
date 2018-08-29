namespace HealthBlog.Common.Trainers.BindingModels
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class ProgramSellBindingModel
    {
		private const string priceDisplay = "Цена";
		private const string priceErrorMessage = "Цената трябва да е положително число.";

		public int Id { get; set; }

		[Required]
		[Range(0, Double.PositiveInfinity, ErrorMessage = priceErrorMessage)]
		[Display(Name = priceDisplay)]
		public decimal Price { get; set; }

		public string Name { get; set; }
	}
}
