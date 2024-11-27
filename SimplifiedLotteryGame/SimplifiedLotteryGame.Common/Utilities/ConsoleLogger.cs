namespace SimplifiedLotteryGame.Common.Utilities
{
    using SimplifiedLotteryGame.Common.Utilities.Contracts;

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log()
        {
            Console.WriteLine();
        }
    }
}
