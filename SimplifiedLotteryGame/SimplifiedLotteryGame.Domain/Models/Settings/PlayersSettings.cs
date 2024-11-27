namespace SimplifiedLotteryGame.Domain.Models.Settings
{
    public class PlayersSettings
    {
        public decimal Balance { get; set; }
        public int MinimumToParticipate { get; set; }
        public int MaximumToParticipate { get; set; }
    }
}
