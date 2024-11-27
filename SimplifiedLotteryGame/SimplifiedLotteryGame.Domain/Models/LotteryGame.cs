namespace SimplifiedLotteryGame.Domain.Models
{
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class LotteryGame() : IGame
    {
        private readonly IList<IPrize> prizes = [];
        private readonly IList<IPlayer> players = [];

        public bool ArePlayersInitialized { get; private set; } = false;
        public decimal Profit { get; private set; }
        public decimal TotalTicketRevenue { get; private set; }
        public IReadOnlyList<IPlayer> Players => players.AsReadOnly();
        public IReadOnlyList<IPrize> Prizes => prizes.AsReadOnly();

        public void SetArePlayersInitialized()
        {
            ArePlayersInitialized = true;
        }

        public void AddPlayer(IPlayer player)
        {
            players.Add(player);
        }

        public void AddPrize(IPrize prize)
        {
            prizes.Add(prize);
        }

        public void SetProfit(decimal prizePool)
        {
            Profit = TotalTicketRevenue - prizePool;
        }

        public void SetTotalTicketRevenue()
        {
            TotalTicketRevenue = Players.SelectMany(p => ((LotteryPlayer)p).Tickets).Sum(t => t.Price);
        }

        public void Reset()
        {
            players.Clear();
            prizes.Clear();
            TotalTicketRevenue = 0;
            Profit = 0;
            ArePlayersInitialized = false;
        }

        public IList<LotteryTicket> GetPlayersWinningTickets()
            => Players
            .OfType<LotteryPlayer>()
            .SelectMany(p => p.Tickets)
            .Cast<LotteryTicket>()
            .Where(t => !t.HasWon)
            .ToList();
    }
}
