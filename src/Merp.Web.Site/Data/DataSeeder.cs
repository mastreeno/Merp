using System;
using Merp.Web.Site.Models;
using Microsoft.AspNetCore.Identity;

namespace Merp.Web.Site.Data
{
    public static partial class DataSeeder
    {
        public static void Seed(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (roleManager != null)
                SeedRoles(roleManager);
            else
                throw new ArgumentNullException(nameof(roleManager));

            if (userManager != null)
                SeedUsers(userManager);
            else
                throw new ArgumentNullException(nameof(userManager));
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Accountancy").Result)
            {
                var roleResult = roleManager.CreateAsync(new IdentityRole("Accountancy")).Result;

                if (!roleResult.Succeeded)
                    throw new InvalidOperationException("Cannot add Accountancy role.");
            }

            if (!roleManager.RoleExistsAsync("Registry").Result)
            {
                var roleResult = roleManager.CreateAsync(new IdentityRole("Registry")).Result;

                if (!roleResult.Succeeded)
                    throw new InvalidOperationException("Cannot add Registry role.");
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync(EMAIL).Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = EMAIL,
                    Email = EMAIL,
                };

                var result = userManager.CreateAsync(user, PASSWORD).Result;

                if (!result.Succeeded)
                    throw new InvalidOperationException($"Cannot add User '{EMAIL}'.");

                result = userManager.AddToRolesAsync(user, new string[] { "Accountancy", "Registry" }).Result;

                if (!result.Succeeded)
                    throw new InvalidOperationException($"Cannot add User '{EMAIL}' to Roles.");
            }
        }
    }
}
