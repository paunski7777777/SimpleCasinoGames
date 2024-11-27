namespace SimplifiedLotteryGame.Common.Utilities.Contracts
{
    public interface IRandomNumberGenerator
    {
        int Next(int minValue, int maxValue);
        int Next(int maxValue);
        int Next();
    }
}
