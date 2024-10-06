using DevBlog.Core.Domain.Content;
using DevBlog.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Infrastructure.DevBlogDbContext
{
    public class DevBlogDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DevBlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostActivityLog> PostActivityLogs { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostInSeries> PostInSeries { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Ghi đè lại tên các bảng trong Identity
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // Tự động gán trường DateCreated = DateTime.Now khi Insert
            // Tự động gán trường DateModified = DateTime.Now khi Update
            var entities = ChangeTracker
                .Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entityItem in entities)
            {
                var dateCreatedProp = entityItem.Entity.GetType().GetProperty("DateCreated");
                
                if (dateCreatedProp != null && entityItem.State == EntityState.Added)
                {
                    dateCreatedProp.SetValue(entityItem.Entity, DateTime.UtcNow);
                }

                var dateModifiedProp = entityItem.Entity.GetType().GetProperty("DateModified");

                if (dateModifiedProp != null && entityItem.State == EntityState.Modified)
                {
                    dateModifiedProp.SetValue(entityItem.Entity, DateTime.UtcNow);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
