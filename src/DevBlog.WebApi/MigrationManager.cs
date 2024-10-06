using DevBlog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevBlog.WebApi
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DevBlogDbContext>())
                {
                    context.Database.Migrate();
                    new DataSeeder().SeedAsync(context).Wait();
                }
            }

            return app;
        }
    }
}
