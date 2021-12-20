using Common;
using System.IO;
using System.Numerics;
using static Utils;

void ProcessInput(string filename, out string algorithm, out string[] image)
{
    var raw = File.ReadLines(filename).ToList();
    algorithm = raw[0];
    image = raw.Skip(2).ToArray();
}

ProcessInput("input.txt", out var algo, out var image);
ProcessInput("test.input.txt", out var testAlgo, out var testImage);
ProcessInput("test2.input.txt", out var testAlgo2, out var testImage2);

const char On = '#';
const char Off = '.';
const int BorderWidth = 2;

static string[] PadIfNeeded(string[] image, char paddingCharacter)
{
    //print(image, name: "before", delim: System.Environment.NewLine, onOwnLine: true);

    var isPopulated = (char item) => item == On;
    var paddingNeeded =
        image[..BorderWidth].Any(line => line.Any(isPopulated)) ||
        image.Any(line => line[..BorderWidth].Any(isPopulated) || line[^BorderWidth..].Any(isPopulated)) ||
        image[^BorderWidth..].Any(line => line.Any(isPopulated));

    //print(paddingNeeded, name: "paddingNeeded");
    if (paddingNeeded)
    {
        var padded = new List<string>();

        var lineWidth = image[0].Length + (2 * BorderWidth);
        var emptyLine = new string(Enumerable.Repeat(paddingCharacter, lineWidth).ToArray());
        var sidePadding = new string(Enumerable.Repeat(paddingCharacter, BorderWidth).ToArray());

        padded.AddRange(Enumerable.Repeat(emptyLine, BorderWidth));
        padded.AddRange(image.Select(line => $"{sidePadding}{line}{sidePadding}"));
        padded.AddRange(Enumerable.Repeat(emptyLine, BorderWidth));

        image = padded.ToArray();
    }

    //print(image, name: "after", delim: System.Environment.NewLine, onOwnLine: true);

    return image;
}

static string[] Enhance(string[] image, string algo, int iterationCount)
{
    var zeroTarget = algo[0];
    var fullTarget = algo[^1];
    var alternates = zeroTarget == On && zeroTarget != fullTarget;

    for (int i = 0; i < iterationCount; i++)
    {
        var fillChar =
            alternates
                ? i % 2 == 1 ? zeroTarget : fullTarget
                : Off;
        image = PadIfNeeded(image, fillChar);

        var width = image[0].Length;
        var height = image.Length;

        var output = new List<string>();

        for (int y = 1; y < height - 1; y++)
        {
            var line = new List<char>();

            for (int x = 1; x < width - 1; x++)
            {
                var line1 = image[y - 1][(x - 1)..(x + 2)];
                var line2 = image[y + 0][(x - 1)..(x + 2)];
                var line3 = image[y + 1][(x - 1)..(x + 2)];

                var bitString = (line1 + line2 + line3).Replace(Off, '0').Replace(On, '1');
                var algoIndex = Convert.ToInt32(bitString, 2);

                line.Add(algo[algoIndex]);
            }

            output.Add(new string(line.ToArray()));
        }

        image = output.ToArray();
    }

    return image;
}

static void Part1(string[] image, string algo)
{
    image = Enhance(image, algo, iterationCount: 2);
    var part1 = image.Select(line => line.Count(item => item == On)).Sum();

    //print(image, delim: System.Environment.NewLine, onOwnLine: true);

    print(part1);
}
//Part1(testImage, testAlgo);
//Part1(testImage2, testAlgo2);
//Part1(image, algo);

static void Part2(string[] image, string algo)
{
    image = Enhance(image, algo, iterationCount: 50);
    var part2 = image.Select(line => line.Count(item => item == On)).Sum();
    print(part2);
}
Part2(image, algo);
