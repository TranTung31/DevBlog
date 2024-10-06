using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DevBlog.Infrastructure
{
    public class DevBlogDbContextFactory : IDesignTimeDbContextFactory<DevBlogDbContext>
    {
        public DevBlogDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DevBlogDbContext>();

            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new DevBlogDbContext(builder.Options);
        }
    }
}
