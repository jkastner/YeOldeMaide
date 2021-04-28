using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeOldeMaide
{
    public class GameRecords
    {
        public List<GameRecordEntry> Records { get; set; } = new List<GameRecordEntry>();

        public string GetDescription()
        {
            if (!Records.Any())
            {
                return "No previous games";
            }
            StringBuilder sb = new StringBuilder();
            foreach (var curRecord in Records)
            {
                Console.WriteLine($"Player {curRecord.Username}");
                Console.WriteLine("Wins");
                foreach (var curWin in curRecord.WinsPerType)
                {
                    Console.WriteLine($"{curWin.Key}: {curWin.Value}");
                }
                Console.WriteLine("Losses");
                foreach (var curLoss in curRecord.LossesPerType)
                {
                    Console.WriteLine($"{curLoss.Key}: {curLoss.Value}");
                }
            }

            return sb.ToString();

        }

        public void AddEntry(GameRecordEntry newEntry)
        {
            var rec = Records.FirstOrDefault(x => x.Username == newEntry.Username);
            if (rec == null)
            {
                Records.Add(newEntry);
                return;
            }

            CombineDictEntry(newEntry.LossesPerType, rec.LossesPerType);
            CombineDictEntry(newEntry.WinsPerType, rec.WinsPerType);


        }

        private void CombineDictEntry(Dictionary<string, int> newDict, Dictionary<string, int> d)
        {
            foreach (var cur in newDict)
            {
                if (d.ContainsKey(cur.Key))
                {
                    d[cur.Key] = d[cur.Key] + 1;
                }
                else
                {
                    d.Add(cur.Key, cur.Value);
                }
            }
        }

        private void CombineDictEntry(Dictionary<string, int> d)
        {

        }
    }

    public class GameRecordEntry
    {
        public string Username { get; set; }
        public Dictionary<string, int> WinsPerType { get; set; }  = new Dictionary<string, int>();
        public Dictionary<string, int> LossesPerType { get; set; }   = new Dictionary<string, int>();
    }
}
