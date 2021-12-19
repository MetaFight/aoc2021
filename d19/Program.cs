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
    var beaconDeltasByScanner = new Dictionary<int, List<double>>();
    foreach (var kvp in beaconsByScanner)
    {
        var scanner = kvp.Key;
        var beacons = kvp.Value;

        var beaconDeltas = new List<double>();
        for (int i = 0; i < beacons.Count; i++)
        {
            for (int j = i + 1; j < beacons.Count; j++)
            {
                var a = beacons[i];
                var b = beacons[j];

                // Consider storing this alongside source vectors to avoid having to look them up again.
                var diffVector = a - b;
                beaconDeltas.Add(diffVector.Length);
            }
        }
        beaconDeltasByScanner[scanner] = beaconDeltas;
    }

    var cartesianProduct = 
        beaconDeltasByScanner
            .SelectMany((left, index) => 
                beaconDeltasByScanner
                    .Skip(index + 1)
                    .Select(right => (scannerA: left.Key, scannerB: right.Key, overlap: left.Value.Intersect(right.Value).Count()))
            )
            .ToList();

    print(cartesianProduct.Count, "candidates");
    print(cartesianProduct.OrderByDescending(item => item.overlap).Take(5), "top5");

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