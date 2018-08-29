namespace HealthBlog.Common.Exceptions
{
	using System;

	public abstract class HealthBlogBaseException : Exception
    {
		protected HealthBlogBaseException(
			string message) 
			: base(message)
		{
		}
	}
}
