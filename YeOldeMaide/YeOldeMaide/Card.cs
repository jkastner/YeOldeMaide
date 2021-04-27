namespace YeOldeMaide
{
    public class Card
    {

        public Card(string curSuite, string curRank)
        {
            this.Suite = curSuite;
            this.Rank = curRank;
        }

        public string Suite { get; }
        public string Rank { get; }

        
        public bool MatchRank(Card other)
        {
            return other.Rank == this.Rank;
        }

        public string ShortDesc => $"{this.Rank[0]}{this.Suite[0]}";
        public string LongDesc => $"{this.Rank} of {this.Suite}";
    }
}