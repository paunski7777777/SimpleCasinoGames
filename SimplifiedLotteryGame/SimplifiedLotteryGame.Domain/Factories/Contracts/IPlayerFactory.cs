namespace SimplifiedLotteryGame.Domain.Factories.Contracts
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(string gameType, string name, decimal balance);
    }
}
