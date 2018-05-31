using System.IO;
using Merp.Web.Site.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Merp.Web.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            // Seed Roles --------------------------------
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var logFactory = host.Services.GetService<ILoggerFactory>();
                var logger = logFactory.CreateLogger("Seed");

                try
                {
                    logger.LogInformation("Trying to seed data...");

                    var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                    var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

                    Data.DataSeeder.Seed(roleManager, userManager);

                    logger.LogInformation("Seed succeeded!");
                }
                catch
                {
                    logger.LogError("Cannot seed data.");
                }
            }
            // -------------------------------------------

            host.Run();
        }
    }
}
