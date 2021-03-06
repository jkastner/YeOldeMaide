using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeOldeMaide
{
    public class CardDeck
    {
        static List<string> cardSuites = new List<string>() { "Clubs", "Hearts", "Spades", "Diamonds" };
        static List<string> cardRanks = new List<string>() { "2", "3", "4", "5", "6","7","8","9","Ten","Jack","Queen","King", "Ace" };
        
        public static IEnumerable<Card> FullDeck()
        {
            foreach(var curSuite in cardSuites)
            {
                foreach (var curRank in cardRanks)
                {
                    yield return new Card(curSuite, curRank);
                }
            }        
        }

        private class ShuffleCardHelper
        {
            public Card Card { get; set; }
            public Guid Id = Guid.NewGuid();
        }

        public static IEnumerable<Card> Shuffle(IEnumerable<Card> deck)
        {
            List<ShuffleCardHelper> shuffleHelper = new List<ShuffleCardHelper>();
            foreach (var cur in deck)
            {
                shuffleHelper.Add(new ShuffleCardHelper(){Card = cur});
            }

            foreach (var cur in shuffleHelper.OrderBy(x => x.Id))
            {
                yield return cur.Card;
            }
        }
    }
}
