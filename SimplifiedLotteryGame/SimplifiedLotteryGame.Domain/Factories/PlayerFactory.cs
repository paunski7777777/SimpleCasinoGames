namespace SimplifiedLotteryGame.Domain.Factories
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class PlayerFactory() : IPlayerFactory
    {
        public IPlayer CreatePlayer(string gameType, string name, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.InvalidPropertyValue, nameof(name), nameof(PlayerFactory), nameof(CreatePlayer)));
            }

            if (balance < 0)
            {
                throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.InvalidPropertyValue, nameof(balance), nameof(PlayerFactory), nameof(CreatePlayer)));
            }

            return gameType switch
            {
                AppConstants.GameTypes.Lottery => new LotteryPlayer(name, balance),
                _ => throw new InvalidOperationException(string.Format(AppConstants.Messages.Errors.InvalidGameType, gameType))
            };
        }
    }
}
