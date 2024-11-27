namespace SimplifiedLotteryGame.Domain.Services.Contracts
{
    using SimplifiedLotteryGame.Domain.Models;

    public interface ILotteryGrandPrizesService
    {
        void GeneratePrizeWinner(LotteryGame game, decimal prizeAmount);
    }
}
