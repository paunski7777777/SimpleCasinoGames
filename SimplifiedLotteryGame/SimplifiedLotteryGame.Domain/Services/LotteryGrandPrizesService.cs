namespace SimplifiedLotteryGame.Domain.Services
{
    using SimplifiedLotteryGame.Common.Utilities.Helpers;
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;

    public class LotteryGrandPrizesService(IRandomNumberGenerator randomNumberGenerator, ILogger logger, IPrizeFactory prizeFactory) : ILotteryGrandPrizesService
    {
        private readonly string prizeType = AppConstants.PrizeTypes.LotteryGrand;

        public void GeneratePrizeWinner(LotteryGame game, decimal prizeAmount)
        {
            try
            {
                IList<LotteryTicket> playerWinningTickets = game.GetPlayersWinningTickets();
                if (playerWinningTickets.Count == 0)
                {
                    logger.Log(string.Format(AppConstants.Messages.PrizeDetermination.NoWinningTickets, prizeType));
                    return;
                }

                int winnerIndex = randomNumberGenerator.Next(playerWinningTickets.Count);

                LotteryTicket winnerTicket = playerWinningTickets[winnerIndex];
                IPrize prize = prizeFactory.CreatePrize(prizeType, prizeAmount, winnerTicket);

                winnerTicket.SetWon();
                winnerTicket.SetPrize(prize);

                game.AddPrize(prize);

                logger.Log(string.Format(AppConstants.Messages.PrizeDetermination.GrandPrizeWin, prizeType, winnerTicket.Player.Name, prizeAmount));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPrizesService), nameof(GeneratePrizeWinner));
            }
        }
    }
}
