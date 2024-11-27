namespace SimplifiedLotteryGame.Domain.Models
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class LotteryPlayer(string name, decimal balance) : IPlayer
    {
        private readonly IList<ITicket> tickets = [];

        public string Name { get; set; } = name;
        public decimal Balance { get; set; } = balance;
        public int TicketsPurchased { get; private set; }
        public IReadOnlyList<ITicket> Tickets => tickets.AsReadOnly();

        public void AddTicket(ITicket ticket)
        {
            tickets.Add(ticket);
        }

        public void DescreaseBalance(decimal amount)
        {
            if (amount > Balance)
            {
                throw new InvalidOperationException(AppConstants.Messages.InsufficientFunds);
            }

            Balance -= amount;
        }

        public void IncreaseTicketsPurchased(int count)
        {
            TicketsPurchased += count;
        }
    }
}
