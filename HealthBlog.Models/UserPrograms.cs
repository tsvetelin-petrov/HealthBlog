namespace HealthBlog.Models
{
	public class UserProgram
    {
		public string UserId { get; set; }
		public User User { get; set; }

		public int ProgramId { get; set; }
		public Program Program { get; set; }
	}
}
