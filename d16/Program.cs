namespace Day16;

using System.IO;
using static Utils;

public static class Program
{
    public static void Main(string[] args)
    {
        //Part1();
        Part2();
    }

    public static void Part1()
    {
        var tests = new[] {
            /*  0  */  "D2FE28",                          // v6, t4, literal = 2021
            /*  1  */  "38006F45291200",                  // v1, t6, literal 10, literal 20 
            /*  2  */  "EE00D40C823060",                  // v7, t3, literal 1, literal 2, literal 3
            /*  3  */  "8A004A801A8002F478",              // 16 = (4 + [ (1 + [ (5 + [6]) ]) ])
            /*  4  */  "620080001611562C8802118E34",      // 12 = (3 + [ (? + [? + ?]) + (? + [? + ?]) ])
            /*  5  */  "C0015000016115A2E0802F182340",    // 23 = (? + [ (? + [? + ?]) + (? + [? + ?]) ])  w/ different OperatorLengthTypeId than above
            /*  6  */  "A0016C880162017C3686B18A3D4780",  // 31 = (? + [ (? + [ (? + [? + ? + ? + ? + ?]) ])  ])
            };

        var input = tests[6];
        input = File.ReadAllText("input.txt");

        var packet = Packet.FromHex(input);

        var allPackets = packet.GetAllPackets();
        //print(allPackets.Count(), "total packet count");

        var part1 = allPackets.Select(item => item.Version).Sum();
        print(part1, "part1");
    }

    public static void Part2()
    {
        var tests = new[] {
            /*  0  */  "C200B40A82",                    // finds the sum of 1 and 2, resulting in the value 3.
            /*  1  */  "04005AC33890",                  // finds the product of 6 and 9, resulting in the value 54.
            /*  2  */  "880086C3E88112",                // finds the minimum of 7, 8, and 9, resulting in the value 7.
            /*  3  */  "CE00C43D881120",                // finds the maximum of 7, 8, and 9, resulting in the value 9.
            /*  4  */  "D8005AC2A8F0",                  // produces 1, because 5 is less than 15.
            /*  5  */  "F600BC2D8F",                    // produces 0, because 5 is not greater than 15.
            /*  6  */  "9C005AC2F8F0",                  // produces 0, because 5 is not equal to 15.
            /*  7  */  "9C0141080250320F1802104A08",    // produces 1, because 1 + 3 = 2 * 2.
        };

        var input = tests[7];
        input = File.ReadAllText("input.txt");

        var packet = Packet.FromHex(input);
        print(packet.Value, "part2");
    }
}
