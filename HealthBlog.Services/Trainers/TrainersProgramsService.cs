namespace HealthBlog.Services.Trainers
{
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Linq;

	using Contracts;
	using HealthBlog.Common.Trainers.BindingModels;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using HealthBlog.Common.Trainers.ViewModels;
	using HealthBlog.Common.Exceptions;

	public class TrainersProgramsService : BaseProgramService, ITrainersProgramsService
	{
		public TrainersProgramsService(
			HealthBlogDbContext dbContext, 
			IMapper mapper,
			UserManager<User> userManager) 
			: base(dbContext, mapper, userManager)
		{
		}

		public async Task DeleteDayAsync(int dayId, int programId, string username)
		{
			var program = await this.GetProgramByIdAsync(programId, username);

			var programDay = await this.DbContext.ProgramDays
				.FirstOrDefaultAsync(pd => pd.DayId == dayId && pd.ProgramId == programId);

			if (programDay == null)
			{
				throw new InvalidProgramDayException();
			}

			this.DbContext.ProgramDays.Remove(programDay);

			await this.DbContext.SaveChangesAsync();
		}

		public async Task SellProgramAsync(int id, ProgramSellBindingModel model, string username)
		{
			var program = await this.GetProgramByIdAsync(id, username);

			program.IsForSale = true;
			program.Price = model.Price;

			await this.DbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<ProgramsForAddingViewModel>> GetAllProgramsForAdding(string username)
		{
			var userId = (await this.GetUserByNamedAsync(username))?.Id;

			var programs = 
				this.Mapper.Map<IEnumerable<ProgramsForAddingViewModel>>(
					(await this.DbContext.Users
						.Include(u => u.CreatedPrograms)
						.FirstAsync(u => u.Id == userId))
							.CreatedPrograms
							.Where(p => p.Name != defaultUserProgramName));

			return programs;
		}

		public async Task<Program> GetProgramByIdAsync(int programId, string username)
		{
			var userId = (await this.GetUserByNamedAsync(username))?.Id;

			var program = (await this.DbContext.Users
				.Include(u => u.CreatedPrograms)
				.FirstAsync(u => u.Id == userId))
					.CreatedPrograms
					.FirstOrDefault(p => p.Id == programId);

			if (program == null)
			{
				throw new InvalidProgramException();
			}

			return program;
		}

		public async Task<IEnumerable<AllProgramsViewModel>> GetAllOwnedProgramsForIndexAsync(string username)
		{
			var user = await this.GetUserByNamedAsync(username);

			var model = this.Mapper.Map<IEnumerable<AllProgramsViewModel>>(
				await this.DbContext.Programs
					.Where(p => p.AuthorId == user.Id)
					.Where(p => p.Name != defaultUserProgramName)
					.Include(p => p.Days)
					.ToListAsync());

			return model;
		}

		public async Task<bool> IsCreatorUserAsync(string username, int id)
		{
			var userId = (await this.GetUserByNamedAsync(username)).Id;

			return await this.DbContext.Programs
				.AnyAsync(p => p.Id == id && p.AuthorId == userId);
		}

		public async Task<ProgramSellBindingModel> GetProgramForSelling(int id, string username)
		{
			if (!await this.IsCreatorUserAsync(username, id))
			{
				throw new InvalidProgramException();
			}

			var program = this.Mapper.Map<ProgramSellBindingModel>(
				await this.DbContext.Programs.FindAsync(id));

			return program;
		}
	}
}
