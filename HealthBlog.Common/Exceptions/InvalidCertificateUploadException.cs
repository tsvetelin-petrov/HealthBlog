namespace HealthBlog.Common.Exceptions
{
	public class InvalidCertificateUploadException : HealthBlogBaseException
	{
		private const string message = "Uploaded file isn't image!";

		public InvalidCertificateUploadException() 
			: base(message)
		{
		}
	}
}
