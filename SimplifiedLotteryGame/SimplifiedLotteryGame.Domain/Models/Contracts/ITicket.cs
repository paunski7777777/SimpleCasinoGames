namespace SimplifiedLotteryGame.Domain.Models.Contracts
{
    public interface ITicket
    {
        Guid Id { get; }
        decimal Price { get; }
        bool HasWon { get; }
        IPlayer Player { get; }
        IPrize? Prize { get; }
    }
}
