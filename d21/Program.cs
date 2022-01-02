using Common;
using Day21;
using System.Numerics;
using static Day21.GameTurn;
using static Utils;

static void Part1()
{
    var input = File.ReadLines("input.txt").ToList();

    var die = new DD100();
    var p1 = new Player() { Id = 1, Position = int.Parse(input[0].Split()[^1]) };
    var p2 = new Player() { Id = 2, Position = int.Parse(input[1].Split()[^1]) };

    var players = new[] { p1, p2 };
    int playerIndex = 0;

    while (p1.Score < 1000 && p2.Score < 1000)
    {
        players[playerIndex].TakeTurn(die);
        playerIndex = (playerIndex + 1) % 2;
    }

    var part1 = players.Min(item => item.Score) * die.RollCount;

    print(part1, "part1");
}
//Part1();


static void Part2()
{

    var input = File.ReadLines("input.txt").ToList();

    var p1 = new PlayerState(int.Parse(input[0].Split()[^1]));
    var p2 = new PlayerState(int.Parse(input[1].Split()[^1]));
    var players = new[] { p1, p2 };

    var root = new GameTurn(players);
    
    print(p1, "player 1");
    print(p2, "player 2");

    using (AutoStopwatch _ = new())
    {
        root.Simulate(out var p1Wins, out var p2Wins);

        print(p1Wins, "p1 actual");
        print(p2Wins, "p2 actual");

        var part2 = Math.Max(p1Wins, p2Wins);
        print(part2, "part2");
    }
}
Part2();
