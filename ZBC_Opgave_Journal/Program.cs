using System;

namespace ZBC_Opgave_Journal
{
    class Program
    {
        static void Main(string[] args)
        {
            JournalLogic logic = new JournalLogic();

            GUI gui = new GUI(logic);
            gui.MainMenu();

            Console.ReadKey();
        }
    }
}
