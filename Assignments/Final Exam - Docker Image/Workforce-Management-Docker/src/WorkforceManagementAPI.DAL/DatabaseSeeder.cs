using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL
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

                    User admin = new User()
                    {
                        UserName = "admin@identity.com",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Id = Guid.NewGuid().ToString("D"),
                        Email = "admin@identity.com",
                        NormalizedEmail = "admin@identity.com".ToUpper(),
                        EmailConfirmed = true,
                        NormalizedUserName = "admin@identity.com".ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString("D"),

                    };

                    admin.PasswordHash = hasher.HashPassword(admin, "adminpass");

                    IdentityRole adminRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityRole userRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "User",
                        NormalizedName = "User".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };


                    IdentityUserRole<string> initialAdminRole = new IdentityUserRole<string>()
                    {
                        RoleId = adminRole.Id,
                        UserId = admin.Id
                    };

                    context.Users.Add(admin);
                    context.Roles.Add(adminRole);
                    context.Roles.Add(userRole);
                    context.UserRoles.Add(initialAdminRole);
                    context.SaveChanges();

                }
            }
        }

    }
}
