using ContainerApp.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerApp.API
{
    internal static class DbContextInterceptorHelpers
    {

        public static IServiceScope InterceptDbContextUpdates(IHost builder)
        {
            var scope = builder.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
            if (!db.Users.Any())
            {
                Task.Run(async () =>
                {
                    await db.Users.AddAsync(new Contracts.Data.Entities.User
                    {
                        EmailAddress = "admin@admin.com",
                        Password = "admin",
                        Role = Contracts.Enum.UserRole.Owner
                    });
                    await db.SaveChangesAsync();
                });
            }

            return scope;
        }
    }
}