namespace HealthBlog.Web
{
	using AutoMapper;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.UI.Services;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	using Filters;
	using HealthBlog.Data;
	using HealthBlog.Models;
	using HealthBlog.Services.Admins.Contracts;
	using HealthBlog.Services.Admins;
	using HealthBlog.Services.Trainers;
	using HealthBlog.Services.Trainers.Contracts;
	using HealthBlog.Services.Users;
	using HealthBlog.Services.Users.Contracts;
	using Services;


	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddIdentity<User, IdentityRole>()
				.AddDefaultUI()
				.AddDefaultTokenProviders()
				.AddEntityFrameworkStores<HealthBlogDbContext>();

			services.AddDbContext<HealthBlogDbContext>(options =>
				options.UseSqlServer(
					this.Configuration.GetConnectionString("HealthBlog")));

			services.Configure<IdentityOptions>(options =>
			{
				options.Password = new PasswordOptions()
				{
					RequiredLength = 4,
					RequiredUniqueChars = 1,
					RequireDigit = true,
					RequireLowercase = false,
					RequireNonAlphanumeric = false,
					RequireUppercase = false
				};
			});

			services.AddAuthentication()
				.AddFacebook(options =>
				{
					options.AppId = this.Configuration["OAuth:Facebook:AppId"];
					options.AppSecret = this.Configuration["OAuth:Facebook:AppSecret"];
				})
				.AddGoogle(options =>
				{
					options.ClientId = this.Configuration["OAuth:Google:ClientId"];
					options.ClientSecret = this.Configuration["OAuth:Google:ClientSecret"];
				});

			services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

			services.AddAutoMapper();

			this.RegisterGlobalServices(services);

			services.AddMvc(options =>
				{
					options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
					options.Filters.Add<GlobalExceptionFilter>();
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.SeedDatabaseAsync();
			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areas",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private void RegisterGlobalServices(IServiceCollection services)
		{
			services.AddSingleton<IEmailSender>(new SendGridEmailService(Configuration.GetSection("SendGrid:ApiKey").Value));
			services.AddScoped<IExercisesService, ExercisesService>();
			services.AddScoped<ITrainingsService, TrainingsService>();
			services.AddScoped<IMealsService, MealsService>();
			services.AddScoped<IDaysService, DaysService>();
			services.AddScoped<ITrainersProgramsService, TrainersProgramsService>();
			services.AddScoped<IUserProgramsService, UserProgramsService>();
			services.AddScoped<ITrainerValidationService, TrainerValidationService>();
			services.AddScoped<IMakeTrainersService, MakeTrainersService>();

			services.AddTransient<TrainingCreatorFilter>();
			services.AddTransient<TrainerProgramCreatorFilter>();
			services.AddTransient<DayCreatorFilter>();
		}
	}
}
