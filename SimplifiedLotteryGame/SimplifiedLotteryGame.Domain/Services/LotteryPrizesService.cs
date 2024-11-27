namespace SimplifiedLotteryGame.Domain.Services
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Common.Utilities.Helpers;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Domain.Models.Settings;
    using SimplifiedLotteryGame.Domain.Services.Contracts;

    public class LotteryPrizesService(PrizesSettings settings, IRandomNumberGenerator randomNumberGenerator, ILogger logger, IPrizeFactory prizeFactory, ILotteryGrandPrizesService grandPrizesService, ILotteryTierPrizesService tierPrizesService) : IPrizesService
    {
        private readonly PrizesSettings settings = settings;
        private readonly IRandomNumberGenerator randomNumberGenerator = randomNumberGenerator;
        private readonly ILogger logger = logger;
        private readonly IPrizeFactory prizeFactory = prizeFactory;
        private readonly ILotteryGrandPrizesService grandLotteryPrizesService = grandPrizesService;
        private readonly ILotteryTierPrizesService tierPrizesService = tierPrizesService;

        public void DeterminePrize(LotteryGame game)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(game, string.Format(AppConstants.Messages.Errors.NullParameter, nameof(game), nameof(LotteryPrizesService), nameof(DeterminePrize)));

                LogPrizeIntro();

                game.SetTotalTicketRevenue();

                decimal grandPrize = CalculatePrize(game.TotalTicketRevenue, settings.GrandPrizePercentage);
                decimal secondTierPrize = CalculatePrize(game.TotalTicketRevenue, settings.SecondTierPrizePercentage);
                decimal thirdTierPrize = CalculatePrize(game.TotalTicketRevenue, settings.ThirdTierPrizePercentage);

                decimal totalPrizePool = grandPrize + secondTierPrize + thirdTierPrize;

                grandLotteryPrizesService.GeneratePrizeWinner(game, grandPrize);
                tierPrizesService.GeneratePrizeWinner(game, secondTierPrize, settings.SecondTierTicketSharePercentage, AppConstants.PrizeTypes.LotterySecondTier);
                tierPrizesService.GeneratePrizeWinner(game, thirdTierPrize, settings.ThirdTierTicketSharePercentage, AppConstants.PrizeTypes.LotteryThirdTier);

                game.SetProfit(totalPrizePool);

                LogPrizeSummary(game);
            }
            catch (ArgumentNullException argumentNullException)
            {
                ExceptionHelper.HandleArgumentNullException(logger, argumentNullException, nameof(LotteryPrizesService), nameof(DeterminePrize));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, invalidOperationException, nameof(LotteryPrizesService), nameof(DeterminePrize));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPrizesService), nameof(DeterminePrize));
            }
        }

        private decimal CalculatePrize(decimal totalTicketRevenue, decimal prize)
        {
            try
            {
                return totalTicketRevenue * prize / AppConstants.PercentDivider;
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPrizesService), nameof(CalculatePrize));
                return 0;
            }
        }

        private void LogPrizeIntro()
        {
            try
            {
                logger.Log(AppConstants.Messages.PrizeDetermination.Results);
                logger.Log();
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPrizesService), nameof(LogPrizeSummary));
            }
        }

        private void LogPrizeSummary(IGame game)
        {
            try
            {
                logger.Log();
                logger.Log(AppConstants.Messages.PrizeDetermination.Congratulations);
                logger.Log();
                logger.Log(string.Format(AppConstants.Messages.PrizeDetermination.HouseRevenue, game.Profit));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPrizesService), nameof(LogPrizeSummary));
            }
        }
    }
}
