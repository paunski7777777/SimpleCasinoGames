namespace SimplifiedLotteryGame.Domain.Models
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class LotteryThirdTierPrize(decimal amount, ITicket ticket) : IPrize
    {
        public ITicket Ticket { get; set; } = ticket;
        public decimal Amount { get; set; } = amount;
    }
}
