using static System.Console;

public static class Utils {
  private static int printNameWidth = 0;
  public static void print<T>(T arg, string name = "") {
    var additionalPaddingRequired = name.Length - printNameWidth;
    if (additionalPaddingRequired > 0) {
      printNameWidth += (int)(1.5 * additionalPaddingRequired);
    }
    
    if (arg is IEnumerable enumerable && arg is not string) {
      var list = enumerable.Cast<object>().ToList();
      var entry = list.FirstOrDefault();
      if (entry is IEnumerable && entry is not string) {
        name = $">{name}";
        list.ForEach(item => print(item, name));
      } else {
        name = $"{name}[]";
        print(string.Join(", ", list), name);
      } 
    } else {
      WriteLine($"{name.PadLeft(printNameWidth)} {arg}");
    }
  }
}