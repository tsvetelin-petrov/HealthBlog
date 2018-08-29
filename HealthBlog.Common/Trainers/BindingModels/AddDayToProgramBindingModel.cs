namespace HealthBlog.Common.Trainers.BindingModels
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using ViewModels;

	public class AddDayToProgramBindingModel
	{
		private const string programDisplayName = "Програма";

		public int DayId { get; set; }

		[Display(Name = programDisplayName)]
		public int ProgramId { get; set; }

		public IEnumerable<ProgramsForAddingViewModel> Programs { get; set; }
	}
}
