using System;
using System.Collections.Generic;

namespace Common
{
    public class Menu
    {
        public Menu()
        {
            MenuItems = new List<MenuItem>();
        }

        public List<MenuItem> MenuItems { get; set; }
        public int MenuId { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Prints the menu items of the Menu to the Console with a number (index)
        /// </summary>
        public void PrintToConsole()
        {
            Console.Clear();

            Console.WriteLine(Description);
            
            Console.WriteLine(string.Empty);

            foreach (var menu in MenuItems)
            {
                Console.WriteLine(MenuItems.IndexOf(menu) + 1 + " : " + menu.Description);
            }

            Console.WriteLine(string.Empty);
        }
    }
}
