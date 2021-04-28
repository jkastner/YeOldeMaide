using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeOldeMaide
{
    class Reporter
    {
        public static int GetInputWithBounds(int max, int min = 1)
        {
            int ret = -1;
            string error = $"Error - please enter an number only, between {min} and {max}";
            while (ret < 0)
            {
                Console.WriteLine($"Select from {min} to {max}");
                var line = Console.ReadLine().Trim();
                if (!int.TryParse(line, out ret))
                {
                    Console.WriteLine(error);
                }

                if (ret > max)
                {
                    ret = -1;
                }

                if (ret < 1)
                {
                    ret = -1;
                }

                if (ret < min)
                {
                    ret = -1;
                }
                if (ret == -1)
                {
                    Console.WriteLine(error);
                }
            }

            return ret;

        }

        public static bool ShowAllDebuggingInfo = false;
        public static void Debug(string s)
        {
            if (ShowAllDebuggingInfo)
            {
                Console.WriteLine("XRAY: "+s);
            }
        }

        public static string GetCardDesc(IEnumerable<Card> c)
        {
            return string.Join(",", c.Select(x=>x.ShortDesc));
        }


        public static void Report(string info)
        {
            Console.WriteLine(info);
        }

        public static void Report(string info, Player p)
        {
            if (p.IsHuman)
            {
                Console.WriteLine(info);
            }
        }
    }
}
