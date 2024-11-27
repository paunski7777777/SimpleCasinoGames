namespace SimplifiedLotteryGame.Domain.Factories.Contracts
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public interface IPrizeFactory
    {
        IPrize CreatePrize(string type, decimal amount, ITicket ticket);
    }
}
