namespace HealthBlog.Data
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	using Models;

	public class HealthBlogDbContext : IdentityDbContext<User>
	{
		public HealthBlogDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Day> Days { get; set; }

		public DbSet<Exercise> Exercises { get; set; }

		public DbSet<Meal> Meals { get; set; }

		public DbSet<MealDay> MealDays { get; set; }

		public DbSet<Training> Trainings { get; set; }

		public DbSet<TrainingDay> TrainingDays { get; set; }

		public DbSet<TrainingExercise> TrainingExercises { get; set; }

		public DbSet<Program> Programs { get; set; }

		public DbSet<UserProgram> UserPrograms { get; set; }

		public DbSet<ProgramDay> ProgramDays { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<TrainingExercise>()
				.HasKey(te => new { te.TrainingId, te.ExerciseId });

			builder.Entity<MealDay>()
				.HasKey(md => new { md.DayId, md.MealId });

			builder.Entity<TrainingDay>()
				.HasKey(td => new { td.DayId, td.TrainingId });

			builder.Entity<UserProgram>()
				.HasKey(up => new { up.UserId, up.ProgramId });

			builder.Entity<ProgramDay>()
				.HasKey(pd => new { pd.DayId, pd.ProgramId });

			builder.Entity<TrainingDay>()
				.HasOne(td => td.Day)
				.WithMany(d => d.Trainings)
				.HasForeignKey(td => td.DayId);

			builder.Entity<Day>()
				.HasMany(d => d.Trainings)
				.WithOne(t => t.Day)
				.HasForeignKey(t => t.DayId);

			builder.Entity<Day>()
				.HasMany(d => d.Meals)
				.WithOne(t => t.Day)
				.HasForeignKey(t => t.DayId);

			builder.Entity<Training>()
				.HasMany(t => t.Days)
				.WithOne(d => d.Training)
				.HasForeignKey(d => d.TrainingId);

			builder.Entity<Meal>()
				.HasMany(t => t.Days)
				.WithOne(d => d.Meal)
				.HasForeignKey(d => d.MealId);

			builder.Entity<Training>()
				.HasMany(t => t.Exercises)
				.WithOne(e => e.Training)
				.HasForeignKey(e => e.TrainingId);

			builder.Entity<Exercise>()
				.HasMany(e => e.Trainings)
				.WithOne(t => t.Exercise)
				.HasForeignKey(t => t.ExerciseId);

			base.OnModelCreating(builder);
		}
	}
}
