using System;
using ToDoListApp.View;
using Microsoft.Extensions.Configuration;
using ToDoListApp.Data;
using ToDoListApp.Services;

namespace ToDoListApp
{
    class Program
    {
        private static Menu _menu;
        private static IConfigurationRoot _configuration;
        static void Main(string[] args)
        {
            InitializeApplication();

            bool shouldexit = false;
            while (!shouldexit)
            {
                shouldexit = _menu.MainMenu();
            }
        }

        static void InitializeApplication()
        {
            _configuration = ConfigInitializer.InitConfig();

            string connectionString = _configuration.GetConnectionString("Default");

            DatabaseContext context = new DatabaseContext(connectionString);
            context.Database.EnsureCreated();

            UserService userService = new UserService(context);
            ToDoListService toDoListService = new ToDoListService(context, userService);
            TaskService taskService = new TaskService(context, userService, toDoListService);

            UserMenu userMenu = new UserMenu(userService);
            ToDoListMenu toDoListMenu = new ToDoListMenu(userService, toDoListService);
            TaskMenu taskMenu = new TaskMenu(userService, toDoListService, taskService);

            _menu = new Menu(userService, toDoListService, taskService, userMenu, toDoListMenu, taskMenu);
        }

    }
    
}
