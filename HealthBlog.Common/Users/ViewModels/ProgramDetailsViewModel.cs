namespace HealthBlog.Common.Users.ViewModels
{
	using System.Collections.Generic;

	public class ProgramDetailsViewModel
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public string Type { get; set; }

		public string Description { get; set; }

		public IEnumerable<AllDaysViewModel> Days { get; set; }

		public bool IsCreatedByCurrentUser { get; set; }
	}
}
