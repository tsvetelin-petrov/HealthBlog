namespace HealthBlog.Web.Mapping
{
	using AutoMapper;

	using HealthBlog.Models;
	using HealthBlog.Common.Admins.ViewModels;
	using HealthBlog.Common.Trainers.BindingModels;
	using HealthBlog.Common.Trainers.ViewModels;
	using HealthBlog.Common.Users.BindingModels;
	using HealthBlog.Common.Users.ViewModels;
	using System;

	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			ConfigureExercises();
			ConfigureTrainings();
			ConfigureMeals();
			ConfigureDays();
			ConfigurePrograms();
			ConfiguteUsers();
		}

		private void ConfiguteUsers()
		{
			this.CreateMap<User, AllTrainerRequestsViewModel>();
		}

		private void ConfigureExercises()
		{
			this.CreateMap<ExerciseCreateBindingModel, Exercise>();
			this.CreateMap<Exercise, AllExercisesViewModel>();
			this.CreateMap<Exercise, ExerciseDetailsViewModel>();
		}

		private void ConfigureTrainings()
		{
			this.CreateMap<TrainingCreateBindingModel, Training>();
			this.CreateMap<TrainingExerciseModel, TrainingExercise>();
			this.CreateMap<Training, TrainingDetailsViewModel>()
				.ForMember(vm => vm.IsCreatedByCurrentUser, opt => opt.Ignore());
			this.CreateMap<TrainingExercise, TrainingDetailsExerciseViewModel>()
				.ForMember(vm => vm.Name, opt => opt.MapFrom(te => te.Exercise.Name))
				.ForMember(vm => vm.TargetMuscle, opt => opt.MapFrom(te => te.Exercise.TargetMuscle))
				.ForMember(vm => vm.Id, opt => opt.MapFrom(te => te.Exercise.Id));
			this.CreateMap<Training, AllTrainingsViewModel>()
				.ForMember(vm => vm.ExerciseCount, opt => opt.MapFrom(t => t.Exercises.Count));
			this.CreateMap<TrainingExerciseInput, TrainingExercise>();
		}

		private void ConfigureMeals()
		{
			this.CreateMap<MealCreateBindingModel, Meal>();
			this.CreateMap<Meal, AllMealsViewModel>()
				.ForMember(vm => vm.Description, options => options.MapFrom(m =>
					m.Description.Length > 20
					? m.Description.Substring(0, 20) + "..."
					: m.Description));
			this.CreateMap<Meal, MealDetailsViewModel>()
				.ForMember(vm => vm.IsCreatedByCurrentUser, opt => opt.Ignore());
		}

		private void ConfigurePrograms()
		{
			this.CreateMap<ProgramCreateBindingModel, Program>();
			this.CreateMap<Program, ProgramDetailsViewModel>()
				.ForMember(vm => vm.IsCreatedByCurrentUser, opt => opt.Ignore());
			this.CreateMap<ProgramDay, AllDaysViewModel>()
				.ForMember(vm => vm.Id, opt => opt.MapFrom(pd => pd.DayId))
				.ForMember(vm => vm.MealsCount, opt => opt.MapFrom(pd => pd.Day.Meals.Count))
				.ForMember(vm => vm.TrainingsCount, opt => opt.MapFrom(pd => pd.Day.Trainings.Count));
			this.CreateMap<Program, AllProgramsViewModel>()
				.ForMember(vm => vm.DaysCount, opt => opt.MapFrom(p => p.Days.Count));
			this.CreateMap<Program, ProgramsIndexViewModel>()
				.ForMember(vm => vm.DaysCount, opt => opt.MapFrom(p => p.Days.Count));
			this.CreateMap<UserProgram, ProgramsIndexViewModel>()
				.ForMember(vm => vm.Id, opt => opt.MapFrom(up => up.ProgramId))
				.ForMember(vm => vm.Name, opt => opt.MapFrom(up => up.Program.Name))
				.ForMember(vm => vm.Type, opt => opt.MapFrom(up => up.Program.Type))
				.ForMember(vm => vm.Author, opt => opt.MapFrom(up => up.Program.Author.UserName))
				.ForMember(vm => vm.DaysCount, opt => opt.MapFrom(up => up.Program.Days.Count));
			this.CreateMap<Program, ProgramSellBindingModel>();
		}

		private void ConfigureDays()
		{
			this.CreateMap<ProgramDay, DayDetailsViewModel>()
				.ForMember(vm => vm.Id, opt => opt.MapFrom(pd => pd.DayId))
				.ForMember(vm => vm.Meals, opt => opt.MapFrom(pd => pd.Day.Meals))
				.ForMember(vm => vm.Trainings, opt => opt.MapFrom(pd => pd.Day.Trainings))
				.ForMember(vm => vm.IsCreatedByCurrentUser, opt => opt.Ignore());
			this.CreateMap<MealDay, DayMealDetailsViewModel>()
				.ForMember(vm => vm.Id, opt => opt.MapFrom(md => md.Meal.Id))
				.ForMember(vm => vm.Name, opt => opt.MapFrom(md => md.Meal.Name));
			this.CreateMap<TrainingDay, DayTrainingDetailsViewModel>()
				.ForMember(vm => vm.Id, opt => opt.MapFrom(md => md.Training.Id))
				.ForMember(vm => vm.Name, opt => opt.MapFrom(md => md.Training.Name))
				.ForMember(vm => vm.Type, opt => opt.MapFrom(md => md.Training.Type));
			this.CreateMap<Day, AllDaysViewModel>()
				.ForMember(vm => vm.TrainingsCount, opt => opt.MapFrom(pd => pd.Trainings.Count))
				.ForMember(vm => vm.MealsCount, opt => opt.MapFrom(pd => pd.Meals.Count));
		}
	}
}
