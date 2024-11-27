namespace SimplifiedLotteryGame.Domain.Services
{
    using SimplifiedLotteryGame.Common.Utilities.Helpers;
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;

    public class LotteryTierPrizesService(IRandomNumberGenerator randomNumberGenerator, ILogger logger, IPrizeFactory prizeFactory) : ILotteryTierPrizesService
    {
        public void GeneratePrizeWinner(LotteryGame game, decimal prizeAmount, decimal sharePercantage, string prizeType)
        {
            try
            {
                IList<LotteryTicket> playersWinningTickets = game.GetPlayersWinningTickets();

                int winnerTicketsCount = (int)Math.Round(playersWinningTickets.Count * sharePercantage / AppConstants.PercentDivider);
                if (winnerTicketsCount == 0)
                {
                    logger.Log(string.Format(AppConstants.Messages.PrizeDetermination.NoWinningTickets, prizeType));
                    return;
                }

                IEnumerable<LotteryTicket> winnerTickets = playersWinningTickets
                   .OrderBy(_ => randomNumberGenerator.Next())
                   .Take(winnerTicketsCount);


                decimal individualPrize = prizeAmount / winnerTicketsCount;

                foreach (LotteryTicket ticket in winnerTickets)
                {
                    IPrize prize = prizeFactory.CreatePrize(prizeType, individualPrize, ticket);

                    ticket.SetWon();
                    ticket.SetPrize(prize);

                    game.AddPrize(prize);
                }

                IEnumerable<string> winningNames = winnerTickets
                    .Select(t => t.Player.Name.Replace($"{AppConstants.PlayerName} ", string.Empty))
                    .Distinct();
                string winningNumbers = string.Join(", ", winningNames);

                logger.Log(string.Format(AppConstants.Messages.PrizeDetermination.TierPrizeWin, prizeType, winningNumbers, individualPrize));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPrizesService), nameof(GeneratePrizeWinner));
            }
        }
    }
}
