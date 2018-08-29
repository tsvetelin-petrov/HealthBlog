namespace HealthBlog.Common.Exceptions
{
	public class InvalidExerciseException : HealthBlogBaseException
	{
		private const string message = "Exercise not found!";

		public InvalidExerciseException() : base(message)
		{
		}
	}
}
