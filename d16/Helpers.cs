using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Day16;
internal static class Helpers
{
	public static string PadHex(string hex) => hex.PadRight(4 * ((hex.Length / 4) + 1), '0');
	public static string ToBitString(IEnumerable<bool> bits) => new String(bits.Select(item => item ? '1' : '0').ToArray());
	public static string ToBitString(string hex)
	  => String.Join(
		  "",
		  Enumerable
			.Range(0, (int)Math.Ceiling(hex.Length / 4f))
			.Select(item => item * 4)
			.Select(offset => Convert.ToString(Convert.ToInt32(hex[offset..Math.Min(hex.Length, offset + 4)], 16), 2).PadLeft(16, '0')));
	public static IEnumerable<bool> ToBits(string hex) => ToBitString(hex).Select(item => item == '1').ToList();
	public static int ToInt(IEnumerable<bool> bits) => bits.Reverse().Select((item, index) => (item ? 1 : 0) << index).Sum();
	public static long ToLong(IEnumerable<bool> bits) => Convert.ToInt64(ToBitString(bits), 2);
}
