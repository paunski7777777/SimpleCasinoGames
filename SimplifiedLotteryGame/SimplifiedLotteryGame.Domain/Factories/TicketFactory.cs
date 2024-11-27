namespace SimplifiedLotteryGame.Domain.Factories
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class TicketFactory() : ITicketFactory
    {
        public ITicket CreateTicket(string gameType, IPlayer player, decimal price)
        {
            if (player == null)
            {
                throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.InvalidPropertyValue, nameof(player), nameof(TicketFactory), nameof(CreateTicket)));
            }

            if (price <= 0)
            {
                throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.InvalidPropertyValue, nameof(price), nameof(TicketFactory), nameof(CreateTicket)));
            }

            return gameType switch
            {
                AppConstants.GameTypes.Lottery => new LotteryTicket(player, price),
                _ => throw new InvalidOperationException(string.Format(AppConstants.Messages.Errors.InvalidGameType, gameType))
            };
        }
    }
}
