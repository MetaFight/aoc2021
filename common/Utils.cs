using System.Collections;
using System.IO;
using static System.Console;

public static class Utils {
  private static int printNameWidth = 0;

  public static string format<T>(T arg, string name = "", string delim = ", ") {
    var additionalPaddingRequired = name.Length - printNameWidth;
    if (additionalPaddingRequired > 0) {
      printNameWidth += (int)(1.5 * additionalPaddingRequired);
    }

    if (arg is IEnumerable enumerable && arg is not string) {
      var list = enumerable.Cast<object>().ToList();
      var entry = list.FirstOrDefault();
      if (entry is IEnumerable && entry is not string) {
        name = $">{name}";
        return string.Join(System.Environment.NewLine, list.Select(item => format(item, name, delim)));
      } else {
        name = $"{name}[]";
        return format(string.Join(delim, list), name);
      } 
    } else {
      return $"{name.PadLeft(printNameWidth)} {arg}";
    }
  }

  public static void print<T>(T arg, string name = "", string delim = ", ")
    => WriteLine(format(arg, name, delim));
  
  public static void clearLog() => File.Delete("out.txt");
  public static void log(string arg) => File.AppendAllText("out.txt", arg);
}