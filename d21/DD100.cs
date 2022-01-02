namespace Day21
{
    internal class DD100 : IDie
    {
        private int lastValue = 0;
        public int RollCount { get; private set; }

        public int Roll()
        {
            int result = (lastValue % 100) + 1;
            this.lastValue = result;
            this.RollCount++;
            return result;
        }
    }
}
