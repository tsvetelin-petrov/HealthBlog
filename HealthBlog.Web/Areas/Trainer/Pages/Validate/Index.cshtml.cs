using HealthBlog.Services.Trainers.Contracts;
using HealthBlog.Common.Constants;
using HealthBlog.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HealthBlog.Web.Areas.Trainer.Pages.Validate
{
	[NotTrainerFilter]
	public class IndexModel : PageModel
    {
		private readonly ITrainerValidationService validationService;

		public IndexModel(ITrainerValidationService validationService)
		{
			this.validationService = validationService;
		}

		[Required]
		[BindProperty]
		public IFormFile SertificateFile { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!this.ModelState.IsValid)
			{
				return this.Page();
			}

			await this.validationService.SubmitCertificatesAsync(this.SertificateFile, this.User.Identity.Name);
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Home, new { area = "" });
		}
    }
}