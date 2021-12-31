public static class Extensions
{
    public static TOut Do<TIn, TOut>(this TIn source, Func<TIn, TOut> func) => func(source);
}