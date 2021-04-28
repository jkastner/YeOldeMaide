using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeOldeMaide
{
    internal class Game
    {
        public Player OldMaid { get; set; }

        public Player Human = new Player() { Name="Human", IsHuman = true};
        List<Player> computerPlayers = new List<Player>();
        List<Player> allPlayers = new List<Player>();
        public Game(int compPlayerCount, string playerName, bool xRay)
        {
            Reporter.ShowAllDebuggingInfo = xRay;
            this.Human.Name = playerName;
            for (int x = 0; x < compPlayerCount; x++)
            {
                this.computerPlayers.Add(new Player(){Name=$"P{x+1}"});
            }
            this.allPlayers.AddRange(this.computerPlayers);
            this.allPlayers.Insert(0, this.Human);
            for (var index = 0; index < this.allPlayers.Count; index++)
            {
                var cur = this.allPlayers[index];
                int n = index + 1;
                if (n == this.allPlayers.Count)
                {
                    n = 0;
                }

                cur.Next = this.allPlayers[n];
            }
        }

        public void DealFirst()
        {
            var shuffled = CardDeck.Shuffle(CardDeck.FullDeck()).ToList();
            Reporter.Debug("Deck order is:");
            Reporter.Debug(Reporter.GetCardDesc(shuffled));
            Reporter.Report("Removing the Old Maid...");
            var oldMaid = shuffled.First(c => c.Rank == "Queen");
            Reporter.Report($"Discarded the {oldMaid.LongDesc}");

            shuffled.Remove(oldMaid);

            var freshDeck = new Stack<Card>(shuffled);
            
            int x = 0;
            while (freshDeck.Any())
            {
                var card = freshDeck.Pop();
                this.allPlayers[x].Hand.Add(card);
                Reporter.Debug($"Dealing {card.ShortDesc} to {this.allPlayers[x].Name}");
                x = x + 1;
                if (x == this.allPlayers.Count)
                {
                    x = 0;
                }
            }

            DisplayPlayerHand();

            foreach (var curPlayer in this.allPlayers)
            {
                Reporter.Debug($"Matching for {curPlayer.Name}");
                this.MakePairs(curPlayer);
            }
        }

        private void MakePairs(Player p)
        {
            if (p.Hand.Count == 0)
            {
                Console.WriteLine($"{p.Name} is out of cards!");
                return;
            }
            foreach (var curCard in p.Hand)
            {
                var allOtherCards = p.Hand.Except(new List<Card>() {curCard});
                var match = allOtherCards.FirstOrDefault(x => x.Rank == curCard.Rank);
                if (match != null)
                {
                    if (p.IsHuman)
                    {
                        Reporter.Report($"You match {curCard.LongDesc} with {match.LongDesc}", p);
                    }
                    if(!p.IsHuman)
                    {
                        Reporter.Debug($"{p.Name} matches {curCard.ShortDesc} with {match.ShortDesc}");
                        Reporter.Report($"{p.Name} creates a match");
                    }
                    
                    p.Matches.Add((curCard, match));
                    p.Hand.Remove(curCard);
                    p.Hand.Remove(match);
                    
                    this.MakePairs(p);
                    break;
                }
                
            }
        }

        public void DebugDisplayCurrentState()
        {
            foreach (var cur in this.allPlayers)
            {
                Reporter.Debug("------");
                Reporter.Debug($"{cur.Name}");
                Reporter.Debug($"Hand {cur.Hand.Count}: {string.Join(", ", cur.Hand.Select(x => x.ShortDesc))}");
                Reporter.Debug("------");
            }
        }

        public void DisplayPlayers()
        {
            Console.WriteLine($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"ROUND {this.Round}");
            Console.WriteLine($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            StringBuilder board = new StringBuilder();
            string boardSide = "------------------";
            string p1 = this.allPlayers[0].BoardDisplay, p2 = $"{this.allPlayers[1].BoardDisplay}";
            string p3 = this.allPlayers.Count > 2 ? this.allPlayers[2].BoardDisplay : "";
            string p4 = this.allPlayers.Count > 3 ? this.allPlayers[3].BoardDisplay : "";

            board.AppendLine($"            {p3}");
            board.AppendLine($"      {boardSide}");
            board.AppendLine("      |                |");
            board.AppendLine("      |                |");
            board.AppendLine("      |                |");
            board.AppendLine($"{p2} |                | {p4}");
            board.AppendLine("      |                |");
            board.AppendLine("      |                |");
            board.AppendLine("      |                |");
            board.AppendLine($"      {boardSide}");
            board.AppendLine($"           {p1}");
            
            Reporter.Report(board.ToString());
            foreach (var curPlayer in this.allPlayers)
            {
                Console.WriteLine($"{curPlayer.Name} is holding {curPlayer.Hand.Count} cards and has created {curPlayer.Matches.Count} pairs");
            }

            DisplayPlayerHand();
            this.DebugDisplayCurrentState();

        }

        private void DisplayPlayerHand()
        {
            Reporter.Report("Your hand is: ");
            Reporter.Report(string.Join(", ", this.Human.Hand.Select(x => x.LongDesc)));
        }


        Random r = new Random();
        public void PromptPlayerForAction(Player p)
        {

            var target = this.FindValidTargetFor(p);
            if (target == null)
            {
                this.HandleLoss(p);
                return;
            }

            Reporter.Report($"Please choose a card from {target.Name}", p);
            int chosenCard = -1;
            if (p.IsHuman)
            {
                chosenCard = Reporter.GetInputWithBounds(target.Hand.Count);
            }
            else
            {
                chosenCard = this.r.Next(0, target.Hand.Count) + 1;
            }
            var cardFromHand = target.Hand[chosenCard - 1];
            Reporter.Report($"{p.Name} has chosen card #{chosenCard} from {target.Name}");
            Reporter.Report($"You have received the {cardFromHand.LongDesc}", p);
            Reporter.Report($"Player {p.Name} has taken {cardFromHand.LongDesc} from your hand", target);
            
            target.Hand.Remove(cardFromHand);
            p.Hand.Add(cardFromHand);
            Reporter.Report($"{p.Name} is making pairs...");
            this.MakePairs(p);
            if (p.IsHuman)
            {
                Console.WriteLine("Press any key to continue the game...");
                Console.ReadKey();
            }
        }

        private Player FindValidTargetFor(Player p)
        {
            
            return this.FindValidTargetFor(p, p.Next);

        }

        private Player FindValidTargetFor(Player org, Player curNext)
        {
            Reporter.Debug($"Looking for target for {org.Name}");
            Reporter.Debug($"Looking at target for {curNext.Name}");
            if (org == curNext)
            {
                Reporter.Debug($"No targets for {org.Name}");
                return null;
            }
            if (curNext.Hand.Count == 0)
            {
                Reporter.Debug($"{curNext.Name} is out of cards");
                return this.FindValidTargetFor(org, curNext.Next);
            }
            Reporter.Debug($"{curNext.Name} is a great target");
            return curNext;
        }

        private void HandleLoss(Player p)
        {
            this.OldMaid = p;
        }

        public int Round { get; set; } = 1;

        public void HandleRound()
        {

            foreach (var curPlayer in this.allPlayers)
            {
                if (curPlayer.Hand.Count > 0)
                {
                    Reporter.Report($"STARTING {curPlayer.Name}'s TURN");

                    this.PromptPlayerForAction(curPlayer);
                }
                else
                {
                    Reporter.Report($"Player {curPlayer.Name} is OUT!");
                }
            }

            this.Round++;
        }
    }
}