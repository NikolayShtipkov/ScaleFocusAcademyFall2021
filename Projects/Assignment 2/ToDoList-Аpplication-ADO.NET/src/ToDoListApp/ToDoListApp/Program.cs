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
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = _menu.MainMenu();
            }
        }

        static void InitializeApplication()
        {
            // Read config file
            _configuration = ConfigInitializer.InitConfig();

            string connectionString = _configuration.GetConnectionString("Default");

            // Create new database and tables 
            //DatabaseInitilizer.InitilizeDatabase(connectionString);

            UserRepository userRepository = new UserRepository(connectionString);
            UserService userService = new UserService(userRepository);


            ToDoListRepository toDoListRepository = new ToDoListRepository(connectionString);
            ToDoListService toDoListService = new ToDoListService(toDoListRepository, userService);

            TaskRepository taskRepository = new TaskRepository(connectionString);
            TaskService taskService = new TaskService(taskRepository, userService, toDoListService);

            UserMenu userMenu = new UserMenu(userService);
            ToDoListMenu toDoListMenu = new ToDoListMenu(userService, toDoListService);
            TaskMenu taskMenu = new TaskMenu(userService, toDoListService, taskService);

            _menu = new Menu(userService, toDoListService, taskService, userMenu, toDoListMenu, taskMenu);
        }

    }
    
}
