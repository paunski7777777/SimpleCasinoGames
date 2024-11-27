namespace SimplifiedLotteryGame.Domain.Factories.Contracts
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public interface IGameFactory
    {
        IGame CreateGame(string type);
    }
}
