using LegalOfficeManagerApp.Data;
using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LegalOfficeManagerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dodanie usług do kontenera
            builder.Services.AddControllersWithViews();

            // Dodanie DbContext z połączeniem do bazy
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Konfiguracja Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
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

            app.UseAuthentication(); // UWAGA! Musisz dodać uwierzytelnianie
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=LogInPage}/{action=Login}/{id?}")
                .WithStaticAssets();

            /*// Dodawanie użytkownika i roli jednorazowo
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                string email = "pies@gmail.com";
                string firstName = "Owczarek";
                string lastName = "Niemiecki";
                string password = "bajojajo3";
                string roleName = "Secretary";
                string phoneNumber = "111111111";
                string gender = "Male";

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("Nie udało się utworzyć roli: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        return;
                    }
                }

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        PhoneNumber = phoneNumber,
                        FirstName = firstName,
                        LastName = lastName,
                        Gender = gender
                    };

                    var createResult = await userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        Console.WriteLine("Nie udalo sie utworzyc użytkownika: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Uzytkownik utworzony.");
                    }
                }

                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, roleName);
                    if (addRoleResult.Succeeded)
                    {
                        Console.WriteLine($"Uzytkownik przypisany do roli '{roleName}'.");
                    }
                    else
                    {
                        Console.WriteLine("Nie udalo sie przypisac roli: " + string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            */
            app.Run();
        }
    }
}
