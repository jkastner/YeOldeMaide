using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeOldeMaide
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("(S)tats or (G)ame?");
            var choice = Console.ReadLine();
            if (choice?.ToLower().Trim() == "s")
            {
                DisplayStats();
            }

            Console.WriteLine("Player name? ");
            var name = Console.ReadLine();
            Console.WriteLine("Number of computer players? ");
            var computerplayerCount = Reporter.GetInputWithBounds(3, 1);
            Console.WriteLine("Activate x-ray vision?");
            var xray = Console.ReadLine();
            bool isXray = xray.ToLower() == "y";
            Game g = new Game(computerplayerCount, name, isXray);
            
            g.DealFirst();
            
            while (g.OldMaid==null)
            {
                g.DisplayPlayers();

                g.HandleRound();
                
            }

            bool won = g.OldMaid != g.Human;
            Console.WriteLine($"Completed - the Old Maid is {g.OldMaid.Name}");
            
            RecordResult(won, g.Human.Name, computerplayerCount);
            DisplayStats();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }

        private static void RecordResult(bool humanWon, string humanName, int cPlayerCount)
        {
            GameRecordEntry re = new GameRecordEntry();
            re.Username = humanName;
            var desc = $"VS {cPlayerCount} Computer Players";
            var tDict = re.LossesPerType;
            if (humanWon)
            {
                tDict = re.WinsPerType;
            }
            tDict.Add(desc, 1);
            GameRecords stats = new GameRecords();
            if (File.Exists(fname))
            {
                var txt = File.ReadAllText(fname);
                stats = JsonConvert.DeserializeObject<GameRecords>(txt);

            }

            stats.AddEntry(re);

            
            var save = JsonConvert.SerializeObject(stats);
            File.WriteAllText(fname, save);
        }

        private const string fname = "record.json";
        private static void DisplayStats()
        {
            if (!File.Exists(fname))
            {
                Console.WriteLine("No previous games");
                return;
            }
            Console.WriteLine("Stats");
            var txt = File.ReadAllText(fname);
            var stats = JsonConvert.DeserializeObject<GameRecords>(txt);
            Console.WriteLine(stats.GetDescription());

        }
    }
}
