namespace SimplifiedLotteryGame.Domain.Services.Contracts
{
    using SimplifiedLotteryGame.Domain.Models;

    public interface IPrizesService
    {
        void DeterminePrize(LotteryGame game);
    }
}
