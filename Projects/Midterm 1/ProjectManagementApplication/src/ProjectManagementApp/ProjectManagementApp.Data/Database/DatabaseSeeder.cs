using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Data.Entities;

namespace ProjectManagementApp.Data.Database
{    public class DatabaseSeeder
    {
        public static void Seed(DatabaseContext context)
        {
            if (context.Database.EnsureCreated())
            {
                context.Users.Add(new User()
                {
                    Username = "admin",
                    Password = "adminpass",
                    FirstName = "SHEfkata",
                    LastName = "GolEma",
                    IsAdmin = true
                });

                context.SaveChanges();
            }
        }
    }
}
