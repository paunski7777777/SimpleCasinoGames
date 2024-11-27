namespace SimplifiedLotteryGame.Common.Utilities
{
    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using System;

    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random random = new();

        public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public int Next()
        {
            return random.Next();
        }
    }
}
