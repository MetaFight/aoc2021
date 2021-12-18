using System.IO;
using static Utils;

Console.WriteLine("Hello, World!");

// var input = File.ReadAllText("test.input.txt");

//var tests = new[] {
//  "[1,2]",
//  "[[1,2],3]",
//  "[9,[8,7]]",
//  "[[1,9],[8,5]]",
//  "[[[[1,2],[3,4]],[[5,6],[7,8]]],9]",
//  "[[[9,[3,8]],[[0,9],6]],[[[3,7],[4,9]],3]]",
//  "[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]",
//};

//var input = tests[6];
//print(input);

//var pair = Pair.Parse(input, out var charsRead);
//print(pair);

/// -------------

//var leftInput = "[[[[4,3],4],4],[7,[[8,4],9]]]";
//var rightInput = "[1,1]";
//var left = Pair.Parse(leftInput, out _);
//var right = Pair.Parse(rightInput, out _);

//var root = new Pair(left, right);
//print(root, "initial");
//while (root.CanReduce)
//{
//    root.Reduce();
//    print(root, "reduced");
//}

/// --------------------

var tests = new[] {
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
};

var root =
    tests.Select(line => Pair.Parse(line, out _))
        .Aggregate((acc, item) => new Pair(acc, item));

print(root, "initial");
int i = 0;
string previous = null;
while (root.CanReduce)
{
    i++;

    root.Reduce();

    var asString = root.ToString();
    if (previous == asString)
    {
    }

    print(asString, $"{i:0000} reduced");
    previous = asString;
}
print(root, "final");