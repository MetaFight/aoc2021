using System.Text.RegularExpressions;
using static Utils;

static List<Step> ExtractInput(string filename)
    => File.ReadLines(filename)
            .Select(line => line.Split().Do(parts => new Step(
                    isOn: parts[0] == "on",
                    cuboid: Regex.Matches(parts[1], "-?\\d+").Do(matches =>
                        new Cuboid(
                                int.Parse(matches[0].Value),
                                int.Parse(matches[1].Value),
                                int.Parse(matches[2].Value),
                                int.Parse(matches[3].Value),
                                int.Parse(matches[4].Value),
                                int.Parse(matches[5].Value)
                        ))
            )))
            .ToList();

List<Step> input = ExtractInput("input.txt");
List<Step> test2Input = ExtractInput("test2.input.txt");

static void Part1(List<Step> input)
{
    var reactor = new List<Voxel>();
    foreach (var step in input)
    {
        var stepVoxels = step.cuboid.GetVoxels(applyPart1Bounds: false);
        if (step.isOn)
        {
            reactor.AddRange(stepVoxels);
        }
        else
        {
            reactor = reactor.Except(stepVoxels).ToList();
        }
    }

    reactor = reactor.Distinct().ToList();

    var part1 = reactor.Count;

    print(part1, "part1");
}
//Part1(input);

static void Part2(List<Step> input)
{
    var steps = input.ToList();

    var onCuboids = FlattenSteps(steps);

    ulong actual;
    checked
    {
        actual = onCuboids.Aggregate(0uL, (acc, item) => acc + item.GetVolume());
    }

    print(actual, "part2");
}
Part2(input);

static HashSet<Cuboid> FlattenSteps(List<Step> input)
{
    var onCuboids = new HashSet<Cuboid>();

    for (int i = 0; i < input.Count; i++)
    {
        var step = input[i];
        var stepCuboid = step.cuboid;

        var toSubdivide = onCuboids.Where(item => item.Overlaps(stepCuboid)).ToList();
        onCuboids.ExceptWith(toSubdivide);

        foreach (var item in toSubdivide)
        {
            var intersection = item.Intersection(stepCuboid);
            onCuboids.UnionWith(item.Except(intersection!));
        }

        if (step.isOn)
        {
            onCuboids.Add(stepCuboid);
        }
    }

    return onCuboids;
}

