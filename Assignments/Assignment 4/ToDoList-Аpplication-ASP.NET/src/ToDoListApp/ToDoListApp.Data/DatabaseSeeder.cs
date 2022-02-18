using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Entities;

namespace ToDoListApp.Data
{    public class DatabaseSeeder
    {
        public static void Seed(DatabaseContext context)
        {
            if (context.Database.EnsureCreated())
            {
                context.Users.Add(new User()
                {
                    Username = "admin",
                    Password = "adminpassword",
                    FirstName = "Admincho",
                    LastName = "Adminkov",
                    IsAdmin = true,
                    CreatorId = 1,
                    ModifierId = 1,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });

                context.SaveChanges();
            }
        }
    }

}
