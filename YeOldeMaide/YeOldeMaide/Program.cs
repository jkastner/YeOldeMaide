using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YeOldeMaide
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Player name? ");
            var name = Console.ReadLine();
            Console.WriteLine("Number of computer players? ");
            var input = Reporter.GetInputWithBounds(3, 1);
            Console.WriteLine("Activate x-ray vision?");
            var xray = Console.ReadLine();
            bool isXray = xray.ToLower() == "y";
            Game g = new Game(input, name, isXray);
            g.DealFirst();
            

            while (g.OldMaid==null)
            {
                g.DisplayPlayers();

                g.HandleRound();
                
            }

            Console.WriteLine($"Completed - the Old Maid is {g.OldMaid.Name}");
            Console.ReadLine();

        }

   
    }
}
