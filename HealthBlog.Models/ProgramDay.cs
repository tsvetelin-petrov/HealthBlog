namespace HealthBlog.Models
{
	public class ProgramDay
	{
		public int ProgramId { get; set; }
		public Program Program { get; set; }

		public int DayId { get; set; }
		public Day Day { get; set; }
	}
}