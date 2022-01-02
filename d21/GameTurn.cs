namespace Day21
{
    record GameTurn
    {
        public record PlayerState(int Position, int Score = 0);

        public static readonly (int value, ushort count)[] rollGroups = new (int value, ushort count)[] {
            (value: 3, count: 1 ),
            (value: 4, count: 3 ),
            (value: 5, count: 6 ),
            (value: 6, count: 7 ),
            (value: 7, count: 6 ),
            (value: 8, count: 3 ),
            (value: 9, count: 1 ),
        };
        private readonly int winningScore;

        public GameTurn(PlayerState[] players, int winningScore = 21, int playerIndex = 0, ushort occurenceCount = 1)
        {
            Players = players;
            this.winningScore = winningScore;
            PlayerIndex = playerIndex % this.Players.Length;
            OccurenceCount = occurenceCount;
        }

        public void Simulate(out ulong p1WinCount, out ulong p2WinCount)
            => this.CalculateWinners(out p1WinCount, out p2WinCount);

        private void CalculateWinners(out ulong p1WinCount, out ulong p2WinCount, int depth = 0)
        {
            if (this.IsP1Winner)
            {
                p1WinCount = this.OccurenceCount;
                p2WinCount = 0;
                return;
            }

            if (this.IsP2Winner)
            {
                p1WinCount = 0;
                p2WinCount = this.OccurenceCount;
                return;
            }

            var nextPlayerIndex =
                depth > 0
                    ? (this.PlayerIndex + 1) % this.Players.Length
                    : this.PlayerIndex;

            ulong p1NextTurnWins = 0;
            ulong p2NextTurnWins = 0;

            foreach (var rollGroup in rollGroups)
            {
                var (rollValue, rollCount) = rollGroup;

                var nextPlayers =
                    this.Players.Select((p, index) =>
                        index == nextPlayerIndex
                            ? p with
                            {
                                Position = CalculateNewPosition(p.Position, rollValue),
                                Score = p.Score + CalculateNewPosition(p.Position, rollValue)
                            }
                            : p).ToArray();

                var nextTurnGroup = this with
                {
                    Players = nextPlayers,
                    PlayerIndex = nextPlayerIndex,
                    OccurenceCount = rollCount
                };


                nextTurnGroup.CalculateWinners(out var p1turnGroupWins, out var p2turnGroupWins, depth + 1);

                p1NextTurnWins += (ulong)this.OccurenceCount * p1turnGroupWins;
                p2NextTurnWins += (ulong)this.OccurenceCount * p2turnGroupWins;
            }

            p1WinCount = p1NextTurnWins;
            p2WinCount = p2NextTurnWins;

            static int CalculateNewPosition(int previousPosition, int rollTotal)
            {
                return ((previousPosition - 1 + rollTotal) % 10) + 1;
            }
        }

        public PlayerState[] Players { get; init; }
        public int PlayerIndex { get; init; }
        public ushort OccurenceCount { get; init; }

        public bool IsP1Winner => this.Players[0].Score >= this.winningScore;
        public bool IsP2Winner => this.Players[1].Score >= this.winningScore;
    }
}
