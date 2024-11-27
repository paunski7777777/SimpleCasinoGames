namespace SimplifiedLotteryGame.Domain.Factories.Contracts
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public interface ITicketFactory
    {
        ITicket CreateTicket(string gameType, IPlayer player, decimal price);
    }
}
