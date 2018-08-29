namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	public class DayCreateBindingModel
    {
		[Required]
		[Range(0, 10)]
		[Display(Name = "Вода за пиене в литри")]
		public double WaterToDrink { get; set; }
	}
}
