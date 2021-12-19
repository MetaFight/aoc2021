namespace Common
{
    public class Compare<T> : IEqualityComparer<T>
    {
        readonly Func<T?, T?, bool> comparer;
        readonly Func<T, int> getHashCode;

        public Compare(Func<T?, T?, bool> comparer, Func<T, int> getHashCode)
        {
            this.comparer = comparer;
            this.getHashCode = getHashCode;
        }

        public bool Equals(T? x, T? y) => this.comparer(x, y);

        public int GetHashCode(T obj) => getHashCode(obj);
    }
}
