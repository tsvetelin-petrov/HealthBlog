namespace HealthBlog.Web
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;

	using HealthBlog.Models;
	using HealthBlog.Common.Constants;

	public static class ApplicationBuilderAuthExtension
    {
		private const string DefaultAdminPassword = "admin123";
		private const string adminUsername = "admin";
		private const string adminEmail = "admin@email.com";

		private static readonly IdentityRole[] roles =
		{
			new IdentityRole(RolesConstants.Administrator),
			new IdentityRole(RolesConstants.Trainer)
		};

		public static async void SeedDatabaseAsync(
			this IApplicationBuilder app)
		{
			var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
			var scope = serviceFactory.CreateScope();

			using (scope)
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

				foreach (var role in roles)
				{
					if (!await roleManager.RoleExistsAsync(role.Name))
					{
						await roleManager.CreateAsync(role);
					}
				}

				var user = await userManager.FindByNameAsync(adminUsername);
				if (user == null)
				{
					user = new User()
					{
						UserName = adminUsername,
						Email = adminEmail
					};

					var result = await userManager.CreateAsync(user, DefaultAdminPassword);

					result = await userManager.AddToRoleAsync(user, roles[0].Name);

					result = await userManager.AddToRoleAsync(user, roles[1].Name);
				}
			}
		}
	}
}
