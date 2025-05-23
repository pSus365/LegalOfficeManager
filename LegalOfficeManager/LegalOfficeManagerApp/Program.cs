using LegalOfficeManagerApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LegalOfficeManagerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dodanie us³ug do kontenera
            builder.Services.AddControllersWithViews();

            // Dodanie DbContext z po³¹czeniem do bazy
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Konfiguracja Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Mo¿esz tu dostosowaæ wymagania has³a (np. na potrzeby testów)
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Konfiguracja cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/LogInPage/Login";
            });

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication(); // <--- UWAGA! Musisz dodaæ uwierzytelnianie
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=LogInPage}/{action=Login}/{id?}")
                .WithStaticAssets();

            // Dodawanie u¿ytkownika i roli jednorazowo
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                string email = "1234";
                string password = "1234"; // silniejsze has³o
                string roleName = "Admin";

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("Nie uda³o siê utworzyæ roli: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        return;
                    }
                }

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        Console.WriteLine("Nie uda³o siê utworzyæ u¿ytkownika: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        return;
                    }
                    else
                    {
                        Console.WriteLine("U¿ytkownik utworzony.");
                    }
                }

                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, roleName);
                    if (addRoleResult.Succeeded)
                    {
                        Console.WriteLine($"U¿ytkownik przypisany do roli '{roleName}'.");
                    }
                    else
                    {
                        Console.WriteLine("Nie uda³o siê przypisaæ roli: " + string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            app.Run();
        }
    }
}
