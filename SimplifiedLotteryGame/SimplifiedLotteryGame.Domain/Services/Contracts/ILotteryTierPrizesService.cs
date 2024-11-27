namespace SimplifiedLotteryGame.Domain.Services.Contracts
{
    using SimplifiedLotteryGame.Domain.Models;

    public interface ILotteryTierPrizesService
    {
        void GeneratePrizeWinner(LotteryGame game, decimal prizeAmount, decimal sharePercantage, string prizeType);
    }
}
