namespace SimplifiedLotteryGame.Common.Utilities
{
    using SimplifiedLotteryGame.Common.Utilities.Contracts;

    public class ConsoleReader : IReader
    {
        public string Read()
        {
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
