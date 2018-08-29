namespace HealthBlog.Models
{
	using Microsoft.AspNetCore.Identity;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class User : IdentityUser
	{
		public User()
		{
			this.OwnedPrograms = new List<UserProgram>();
			this.Trainings = new List<Training>();
			this.Meals = new List<Meal>();
			this.Exercises = new List<Exercise>();
			this.CreatedPrograms = new List<Program>();
		}

		public string FullName { get; set; }

		[DataType(DataType.ImageUrl)]
		public string ImageUrl { get; set; }

		public string CertificatePath { get; set; }

		public bool IsResponded { get; set; }

		public int CertificateUploadTimes { get; set; }

		public ICollection<Program> CreatedPrograms { get; set; }

		public ICollection<UserProgram> OwnedPrograms { get; set; }

		public ICollection<Training> Trainings { get; set; }

		public ICollection<Meal> Meals { get; set; }

		public ICollection<Exercise> Exercises { get; set; }
	}
}
