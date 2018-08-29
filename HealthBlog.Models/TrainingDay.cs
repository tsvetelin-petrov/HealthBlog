using System;
using System.ComponentModel.DataAnnotations;

namespace HealthBlog.Models
{
	public class TrainingDay
	{
		public int TrainingId { get; set; }
		public Training Training { get; set; }

		public int DayId { get; set; }
		public Day Day { get; set; }
	}
}