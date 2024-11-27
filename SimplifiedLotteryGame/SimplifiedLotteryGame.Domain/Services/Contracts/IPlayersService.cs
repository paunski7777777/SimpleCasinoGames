namespace SimplifiedLotteryGame.Domain.Services.Contracts
{
    using SimplifiedLotteryGame.Domain.Models;

    public interface IPlayersService
    {
        void InitializePlayers(LotteryGame game);
    }
}
