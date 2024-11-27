namespace SimplifiedLotteryGame.Domain.Models.Settings
{
    public class PrizesSettings 
    {
        public decimal GrandPrizePercentage { get; set; }
        public decimal SecondTierPrizePercentage { get; set; }
        public decimal ThirdTierPrizePercentage { get; set; }
        public decimal SecondTierTicketSharePercentage { get; set; }
        public decimal ThirdTierTicketSharePercentage { get; set; }
    }
}
