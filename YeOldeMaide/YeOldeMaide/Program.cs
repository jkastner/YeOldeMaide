using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeOldeMaide
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Number of players?");
            var input = ConsoleHelper.GetInputWithBounds(4);
            Game g = new Game(input);
            g.DealFirst();
            Console.ReadLine();

        }

   
    }

    internal class Game
    {
        Player Human = new Player();
        List<Player> players = new List<Player>();
        public Game(int playerCount)
        {
            int npcPlayers = playerCount - 1;
            for (int x = 0; x < npcPlayers; x++)
            {
                this.players.Add(new Player(){Name=$"Player {x+1}"});
            }
            this.players.Add(this.Human);
        }

        public void DealFirst()
        {
            var freshDeck = new Stack<Card>(CardDeck.FullDeck());
            int x = 0;
            while (freshDeck.Any())
            {
                var card = freshDeck.Pop();
                players[x].Hand.Add(card);
                ConsoleHelper.Debug($"Dealing {card.ShortDesc} to {this.players[x].Name}");
                x = x + 1;
                if (x == this.players.Count)
                {
                    x = 0;
                }
                
                
            }

        }
    }

    internal class Player
    {
        public List<Card> Hand { get; } = new List<Card>();
        public string Name { get; set; }
    }
}
