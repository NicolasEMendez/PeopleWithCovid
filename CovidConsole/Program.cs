using Common;
using System;

namespace CovidConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuCreator menuCreator = new MenuCreator();
            CollectionMenu collectionMenu = menuCreator.CreateCollectionMenu();

            collectionMenu.ShowMenu(0);

            Console.ReadKey(true);
        }
    }
}
