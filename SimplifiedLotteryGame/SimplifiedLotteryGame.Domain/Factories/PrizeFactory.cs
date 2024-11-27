namespace SimplifiedLotteryGame.Domain.Factories
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    public class PrizeFactory : IPrizeFactory
    {
        public IPrize CreatePrize(string type, decimal amount, ITicket ticket)
        {
            if (amount <= 0)
            {
                throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.InvalidPropertyValue, nameof(amount), nameof(PrizeFactory), nameof(CreatePrize)));
            }

            if (ticket == null)
            {
                throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.InvalidPropertyValue, nameof(ticket), nameof(PrizeFactory), nameof(CreatePrize)));
            }

            return type switch
            {
                AppConstants.PrizeTypes.LotteryGrand => new LotteryGrandPrize(amount, ticket),
                AppConstants.PrizeTypes.LotterySecondTier => new LotterySecondTierPrize(amount, ticket),
                AppConstants.PrizeTypes.LotteryThirdTier => new LotteryThirdTierPrize(amount, ticket),
                _ => throw new InvalidOperationException(string.Format(AppConstants.Messages.Errors.InvalidPrizeType, type))
            };
        }
    }
}
