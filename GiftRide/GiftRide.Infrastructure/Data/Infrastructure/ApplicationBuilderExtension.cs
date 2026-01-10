using GiftRide.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRide.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            RoleSeeder(services).GetAwaiter().GetResult();
            SeedAdministrator(services).GetAwaiter().GetResult();

            var context = services.GetRequiredService<ApplicationDbContext>();
            SeedCategories(context);
            SeedValidities(context);

            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Administrator", "Client" };
            IdentityResult roleResult;
            foreach (var role in roleNames)
            {
                var roleExit = await roleManager.RoleExistsAsync(role);
                if (!roleExit)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }

            }
        }
        private static async Task SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "admin";
                user.LastName = "admin";
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.Address = "admin address";
                user.PhoneNumber = "0888888888";

                var result = await userManager.CreateAsync
                    (user, "Admin123456");
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }

        }
        private static void SeedCategories(ApplicationDbContext dataCategory)
        {
            if (dataCategory.Categories.Any())
            {
                return;
            }
            dataCategory.Categories.AddRange(new[]
            {
            new Category {CategoryName="Въздушни"},
            new Category {CategoryName="Водни"},
            new Category {CategoryName="Офроуд и Моторни"},
            new Category {CategoryName="Спорт и Природа"},
            new Category {CategoryName="Зимни"}

            });
            dataCategory.SaveChanges();

        }
        private static void SeedValidities(ApplicationDbContext dataValidity )
        {
            if (dataValidity.Validities.Any())
            {
                return;
            }
            dataValidity.Validities.AddRange(new[]
            {
                new Validity {ValidityName="3 месеца"},
                new Validity {ValidityName="6 месеца"},
                new Validity {ValidityName="12 месеца"}
            });
            dataValidity.SaveChanges();
        }

    }
}
