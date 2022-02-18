using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthApp.DAL.Entities;

namespace UserAuthApp.DAL.Data
{
    public class DatabaseSeeder
    {
        public static void Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();

                if (context.Database.EnsureCreated())
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    User user = new User()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Email = "admin@test.com",
                        NormalizedEmail = "admin@test.com".ToUpper(),
                        EmailConfirmed = true,
                        UserName = "admin",
                        NormalizedUserName = "admin".ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };

                    user.PasswordHash = hasher.HashPassword(user, "adminpassword");

                    IdentityRole identityRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityUserRole<string> identityUserRole = new IdentityUserRole<string>() { RoleId = identityRole.Id, UserId = user.Id };

                    context.Roles.Add(identityRole);
                    context.Users.Add(user);
                    context.UserRoles.Add(identityUserRole);

                    context.SaveChanges();
                }
            }
        }
    }
}
