using System;
using ToDoListApp.View;

namespace ToDoListApp
{
    class Program
    {
        private static Menu _menu = new Menu();

        static void Main(string[] args)
        {
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = _menu.MainMenu();
            }
        }

    }
    
}
