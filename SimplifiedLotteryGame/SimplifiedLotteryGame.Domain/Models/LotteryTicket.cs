namespace SimplifiedLotteryGame.Domain.Models
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class LotteryTicket(IPlayer player, decimal price) : ITicket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Price { get; set; } = price;
        public bool HasWon { get; set; } = false;
        public IPlayer Player { get; set; } = player;
        public IPrize? Prize { get; set; }

        public void SetPrize(IPrize prize)
        {
            Prize = prize;
        }

        public void SetWon()
        {
            HasWon = true;
        }
    }
}
