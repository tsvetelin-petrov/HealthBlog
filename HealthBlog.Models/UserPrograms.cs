using System;
using System.ComponentModel.DataAnnotations;

namespace HealthBlog.Models
{
	public class UserProgram
    {
		public int UserId { get; set; }
		public User User { get; set; }

		public int ProgramId { get; set; }
		public Program Program { get; set; }
	}
}
