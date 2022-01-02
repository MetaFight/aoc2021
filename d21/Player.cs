namespace Day21
{
    internal class Player
    {
        public int Id { get; init; }
        public int Position { get; set; }
        public int Score { get; set; }

        public void TakeTurn(IDie die)
        {
            var roll1 = die.Roll();
            var roll2 = die.Roll();
            var roll3 = die.Roll();
            var rollTotal = roll1 + roll2 + roll3;

            this.Position = ((this.Position - 1 + rollTotal) % 10) + 1;
            this.Score += this.Position;

            // print($"Player {this.Id} rolls {roll1}+{roll2}+{roll3} and moves to space {this.Position} for a total score of {this.Score}.");
        }
    }
}
