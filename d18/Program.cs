﻿using System.IO;
using static Utils;

var testInput = new[] {
    /*
    "[1,1]",
    "[2,2]",
    "[3,3]",
    "[4,4]",
    //*/

    /*
    "[1,1]",
    "[2,2]",
    "[3,3]",
    "[4,4]",
    "[5,5]",
    //*/

    /*
    "[1,1]",
    "[2,2]",
    "[3,3]",
    "[4,4]",
    "[5,5]",
    "[6,6]",
    //*/

    /*
    "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
    "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
    "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
    "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
    "[7,[5,[[3,8],[1,4]]]]",
    "[[2,[2,2]],[8,[8,1]]]",
    "[2,9]",
    "[1,[[[9,3],9],[[9,0],[0,7]]]]",
    "[[[5,[7,4]],7],1]",
    "[[[[4,2],2],6],[8,7]]",
    //*/

    //*
    "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
    "[[[5,[2,8]],4],[5,[[9,9],0]]]",
    "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
    "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
    "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
    "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
    "[[[[5,4],[7,7]],8],[[8,3],8]]",
    "[[9,3],[[9,9],[6,[4,9]]]]",
    "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
    "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
    //*/
};
var input = File.ReadLines("input.txt");

static void Part1(IEnumerable<string> input)
{
    var items = input.Select(text => Pair.Parse(text, out _)).ToList();
    var current = items[0];

    print(current, "initial");

    foreach (var next in items.Skip(1))
    {
        current = new Pair(current, next);
        while (current.CanReduce) { current.Reduce(); }
    }
    print(current, "result");
    print(current.Magnitude, "part1");
}
Part1(input);

static void Part2(IEnumerable<string> input)
{
    var pairs = input.SelectMany(left => input.Where(right => right != left).Select(right => new Pair(Pair.Parse(left, out _), Pair.Parse(right, out _)))).ToList();
    foreach (var candidate in pairs) {
        while(candidate.CanReduce) { candidate.Reduce(); }
    }
    pairs = pairs.OrderByDescending(pair => pair.Magnitude).ToList();

    var best = pairs[0];
    print(best);

    print(best.Magnitude, "part2");
}
Part2(input);