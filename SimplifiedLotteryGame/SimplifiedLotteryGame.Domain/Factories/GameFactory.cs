namespace SimplifiedLotteryGame.Domain.Factories
{
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Common;

    public class GameFactory() : IGameFactory
    {
        public IGame CreateGame(string type)
        {
            return type switch
            {
                AppConstants.GameTypes.Lottery => new LotteryGame(),
                _ => throw new InvalidOperationException(string.Format(AppConstants.Messages.Errors.InvalidGameType, type))
            };
        }
    }
}
