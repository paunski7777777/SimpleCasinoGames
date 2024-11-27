namespace SimplifiedLotteryGame.Domain.Models.Settings
{
    public class TicketsSettings
    {
        public decimal Price { get; set; }
        public int MinimumToPurchase { get; set; }
        public int MaximumToPurchase { get; set; }
    }
}
