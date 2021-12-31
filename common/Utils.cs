using System.Collections;
using System.IO;
using static System.Console;

public static class Utils
{
    private static int printNameWidth = 0;

    public static string format<T>(T arg, string name = "", string delim = ", ", bool onOwnLine = false)
    {
        var additionalPaddingRequired = name.Length - printNameWidth;
        if (additionalPaddingRequired > 0)
        {
            printNameWidth += (int)(1.5 * additionalPaddingRequired);
        }

        if (arg is IEnumerable enumerable && arg is not string)
        {
            var list = enumerable.Cast<object>().ToList();
            var entry = list.FirstOrDefault();
            if (entry is IEnumerable && entry is not string)
            {
                name = $">{name}";
                return string.Join(System.Environment.NewLine, list.Select(item => format(item, name, delim, onOwnLine)));
            }
            else
            {
                name = $"{name}[]";
                return format(string.Join(delim, list), name, delim, onOwnLine);
            }
        }
        else
        {
            return $"{name.PadLeft(printNameWidth)}{(onOwnLine ? System.Environment.NewLine : " ")}{arg}";
        }
    }

    public static void print<T>(T arg, string name = "", string delim = ", ", bool onOwnLine = false)
      => WriteLine(format(arg, name, delim, onOwnLine));

    public static void clearLog() => File.Delete("out.txt");
    public static void log(string arg) => File.AppendAllText("out.txt", arg);
}