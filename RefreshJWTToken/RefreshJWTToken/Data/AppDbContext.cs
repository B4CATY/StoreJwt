using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RefreshJWTToken.Models;

namespace RefreshJWTToken.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
            if (Database.EnsureCreated())
            {
				Roles.Add(new IdentityRole { Id = "30859930-c58b-4a91-9970-1658f4d3bdc1", Name = "admin", NormalizedName = "ADMIN" });
				Roles.Add(new IdentityRole {Id = "eadfe780-0e91-4ccd-96a6-1fa53706cf35", Name = "user", NormalizedName = "USER" });
				
				Users.Add(new IdentityUser {
					Id = "6dab7465-c5f1-4383-bd03-8cbf9cb56b77",
					UserName = "admin",
					Email = "admin@gmail.com", 
					PasswordHash = "AQAAAAEAACcQAAAAEB18PUdLSxXAofBXAiopXuuw3qZme89uzHl0EbVcYH1BExRrWnWEg08kRdSTzCFsvg==",
					ConcurrencyStamp = "9cc5c5dd-d7d0-4582-a7df-5ad49c40ed2e",
					SecurityStamp = "XPRPOQI44OWN6DGXTXFFSKFRXZSZ6SHN",
					EmailConfirmed = false,
					NormalizedUserName = "ADMIN",
					NormalizedEmail = "ADMIN@GMAIL.COM",
					PhoneNumber = null,
					TwoFactorEnabled = false,
					PhoneNumberConfirmed = false,
					AccessFailedCount = 0,
					LockoutEnd = null,
					LockoutEnabled = true

				});
				
				UserRoles.Add(new IdentityUserRole<string> { RoleId = "30859930-c58b-4a91-9970-1658f4d3bdc1", UserId = "6dab7465-c5f1-4383-bd03-8cbf9cb56b77" });
				SaveChanges();

			}

		}

		public virtual DbSet<UserRefreshTokens> UserRefreshToken { get; set; }
	}
}
