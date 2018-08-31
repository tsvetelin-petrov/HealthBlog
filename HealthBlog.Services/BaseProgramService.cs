using System.Threading.Tasks;
using AutoMapper;
using HealthBlog.Common.Trainers.BindingModels;
using HealthBlog.Common.Users.BindingModels;
using HealthBlog.Data;
using HealthBlog.Models;
using HealthBlog.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace HealthBlog.Services
{
	public abstract class BaseProgramService : BaseEFService, ICreateProgram
	{
		protected const string defaultUserProgramName = "%^_default_program_name_^%";
		protected const string defaultUserProgramType = "%^_default_program_type_^%";
		protected const string defaultUserProgramDescription = "%^_default_program_description_^%";

		protected BaseProgramService(
			HealthBlogDbContext dbContext,
			IMapper mapper,
			UserManager<User> userManager)
			: base(dbContext, mapper, userManager)
		{
		}

		public async Task CreateProgramAsync(ProgramCreateBindingModel model, string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var program = this.Mapper.Map<Program>(model);
			program.AuthorId = user.Id;
			user.CreatedPrograms.Add(program);
			await this.DbContext.SaveChangesAsync();
		}
	}
}
