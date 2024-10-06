using DevBlog.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace DevBlog.Infrastructure
{
    public class DataSeeder
    {
        public async Task SeedAsync(DevBlogDbContext context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var rootAdminRoleId = Guid.NewGuid();

            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new AppRole
                {
                    Id = rootAdminRoleId,
                    Name = "admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "Quản trị viên"
                });

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var userId = Guid.NewGuid();
                var user = new AppUser
                {
                    Id = userId,
                    FirstName = "Tung",
                    LastName = "Tran",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "trantungdev3105@gmail.com",
                    NormalizedEmail = "TRANTUNGDEV3105@GMAIL.COM",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                };
                user.PasswordHash = passwordHasher.HashPassword(user, "@Admin123");
                
                await context.Users.AddAsync(user);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = rootAdminRoleId,
                    UserId = userId,
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
