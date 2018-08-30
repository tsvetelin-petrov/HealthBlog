namespace HealthBlog.Common.Trainers.BindingModels
{
	using Microsoft.AspNetCore.Mvc.Rendering;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using Constants;

	public class AddDayToProgramBindingModel
	{
		public int DayId { get; set; }

		[Display(Name = AttributeDisplayNameConstants.Program)]
		public int ProgramId { get; set; }

		public IEnumerable<SelectListItem> Programs { get; set; }
	}
}
