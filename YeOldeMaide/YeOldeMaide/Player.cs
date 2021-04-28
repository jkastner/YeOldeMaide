using System.Collections.Generic;
using System.Linq;

namespace YeOldeMaide
{
    internal class Player
    {
        public bool IsHuman { get; set; }
        public List<(Card a, Card b)> Matches { get; } = new List<(Card a, Card b)>();
        public List<Card> Hand { get; } = new List<Card>();
        public string Name { get; set; }
        public Player Next { get; set; }

        public string Mood
        {
            get
            {
                if (Hand.Any(x => x.Rank == "Queen"))
                {
                    return ":-(";
                }

                if (Hand.Count == 0)
                {
                    return ":-D";
                }
                return ":-|";
            }
        }

        public string BoardDisplay
        {
            get
            {
                if (this.IsHuman)
                {
                    return Name;
                }
                else
                {
                    return this.Name.Substring(0, 2) + this.Mood;
                }
            }
        }
    }
}