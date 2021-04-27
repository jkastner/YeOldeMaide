using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeOldeMaide
{
    class ConsoleHelper
    {
        public static int GetInputWithBounds(int max)
        {
            int ret = -1;
            string error = $"Error - please enter an number only, between 1 and {max}";
            while (ret < 0)
            {
                Console.WriteLine($"Select from 1 to {max}");
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

                if (ret == -1)
                {
                    Console.WriteLine(error);
                }
            }

            return ret;

        }

        public const bool Debugging = true;
        public static void Debug(string s)
        {
            if(Debugging)
            Console.WriteLine(s);
        }
    }
}
