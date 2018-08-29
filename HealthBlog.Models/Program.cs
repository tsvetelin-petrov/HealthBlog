namespace HealthBlog.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Program
    {
		public Program()
		{
			this.Days = new List<ProgramDay>();
			this.Users = new List<UserProgram>();
		}

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public string Description { get; set; }

		public string AuthorId { get; set; }
		public User Author { get; set; }

		public bool IsForSale { get; set; }

		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		public ICollection<UserProgram> Users { get; set; }

		public ICollection<ProgramDay> Days { get; set; }
	}
}
