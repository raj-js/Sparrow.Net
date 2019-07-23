using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sparrow.Security.Models;

namespace Sparrow.Security.Data
{
    public class IdentityDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserClaim>()
                .HasOne(s => s.User)
                .WithMany(s => s.UserClaims)
                .HasForeignKey(s => s.UserId);

            builder.Entity<UserRole>()
                .HasOne(s => s.User)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(s => s.UserId);

            builder.Entity<UserRole>()
                .HasOne(s => s.Role)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(s => s.RoleId);

            builder.Entity<RoleClaim>()
                .HasOne(s => s.Role)
                .WithMany(s => s.RoleClaims)
                .HasForeignKey(s => s.RoleId);
        }
    }
}
