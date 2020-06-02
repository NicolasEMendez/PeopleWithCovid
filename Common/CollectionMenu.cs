using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class CollectionMenu
    {
        public CollectionMenu()
        {
            Menus = new List<Menu>();
        }

        public List<Menu> Menus { get; set; }
        public string SelectOption { get; set; }
        public string InvalidSelection { get; set; }

        /// <summary>
        /// Shows the menu based on the id
        /// </summary>
        /// <param name="id">The id of the Menu</param>
        public void ShowMenu(int id)
        {
            var presentMenu = Menus
                .Where(m => m.MenuId == id)
                .Single();

            presentMenu.PrintToConsole();

            Console.WriteLine($"{SelectOption}");

            string selectedOption = Console.ReadLine();

            if (!int.TryParse(selectedOption, out int optionIndex) || optionIndex < 0 || optionIndex > presentMenu.MenuItems.Count)
            {
                Console.Clear();

                Console.WriteLine($"{InvalidSelection}");

                ShowMenu(id);
            }
            else
            {
                var selectedMenuItem = presentMenu.MenuItems[optionIndex - 1];

                Console.Clear();

                if(selectedMenuItem.Actions != null)
                {
                    foreach (var action in selectedMenuItem.Actions)
                    {
                        action.Invoke();
                    }
                }


                if(selectedMenuItem.SubMenuId.HasValue)
                {
                    ShowMenu(selectedMenuItem.SubMenuId.Value);
                }
                
            }
        }

    }
}
