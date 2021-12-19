using Common;
using System.IO;
using static Utils;

var process =
    (string text) =>
        text.Split("\r\n\r\n")
            .Select(scannerBlock =>
                scannerBlock
                    .Split("\r\n")
                    .Skip(1)
                    .Select(beacon => beacon.Split(","))
                    .Select(tokens => new Vector3(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2])))
                    .ToList()
            )
            .Select((item, index) => (scanner: index, beacons: item))
            .ToDictionary(item => item.scanner, item => item.beacons);

var input = process(File.ReadAllText("input.txt"));
var testInput = process(File.ReadAllText("test.input.txt"));

static void Part1(Dictionary<int, List<Vector3>> beaconsByScanner)
{
    var beaconPairsByScanner =
        beaconsByScanner.ToDictionary(
            kvp => kvp.Key, 
            kvp => kvp.Value.SelectMany((left, index) => kvp.Value.Skip(index + 1).Select(right => new BeaconPair(left, right))).ToList());

    var cartesianProduct = 
        beaconPairsByScanner
            .SelectMany((left, index) => 
                beaconPairsByScanner
                    .Skip(index + 1)
                    .Select(right => 
                        (
                            scannerA: left.Key,
                            scannerABeacons: left.Value,
                            scannerB: right.Key,
                            scannerBBeacons: right.Value,
                            overlap: left.Value.Select(pair => pair.Distance).Intersect(right.Value.Select(pair => pair.Distance)).Count()
                        )
                    )
            )
            .ToList();

    print(cartesianProduct.Count, "candidates");
    print(cartesianProduct.OrderByDescending(item => item.overlap).Take(5).Select(item => (item.scannerA, item.scannerB, item.overlap)), "top5");

    //print(current.Magnitude, "part1");
}
Part1(testInput);

static void Part2(Dictionary<int, List<Vector3>> beaconsByScanner)
{

    //print(best.Magnitude, "part2");
}
Part2(input);

record Vector3(int x, int y, int z)
{
    public static Vector3 operator -(Vector3 leftHand, Vector3 rightHand)
        => new Vector3(leftHand.x - rightHand.x, leftHand.y - rightHand.y, leftHand.z - rightHand.z);

    public double Length => Math.Sqrt((x * x) + (y * y) + (z * z));
}

record BeaconPair(Vector3 a, Vector3 b)
{
    public double Distance = (a - b).Length;
}