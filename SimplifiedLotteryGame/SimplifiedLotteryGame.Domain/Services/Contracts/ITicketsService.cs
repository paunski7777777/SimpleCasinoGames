namespace SimplifiedLotteryGame.Domain.Services.Contracts
{
    using SimplifiedLotteryGame.Domain.Models;

    public interface ITicketsService
    {
        void PurchaseTickets(LotteryGame game);
    }
}
