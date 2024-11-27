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

    public class LotteryTicketsService(TicketsSettings settings, IRandomNumberGenerator randomNumberGenerator, IReader reader, ILogger logger, ITicketFactory ticketFactory) : ITicketsService
    {
        private const string gameType = AppConstants.GameTypes.Lottery;
        private readonly TicketsSettings settings = settings;
        private readonly IRandomNumberGenerator randomNumberGenerator = randomNumberGenerator;
        private readonly IReader reader = reader;
        private readonly ILogger logger = logger;
        private readonly ITicketFactory ticketFactory = ticketFactory;

        public void PurchaseTickets(LotteryGame game)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(game, string.Format(AppConstants.Messages.Errors.NullParameter, nameof(game), nameof(LotteryTicketsService), nameof(PurchaseTickets)));

                foreach (LotteryPlayer player in game.Players.Cast<LotteryPlayer>())
                {
                    ProcessPlayerTickets(player);
                }

                LogPurchaseSummary(game);
            }
            catch (ArgumentNullException argumentNullException)
            {
                ExceptionHelper.HandleArgumentNullException(logger, argumentNullException, nameof(LotteryTicketsService), nameof(PurchaseTickets));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryTicketsService), nameof(PurchaseTickets));
            }
        }

        private void ProcessPlayerTickets(LotteryPlayer player)
        {
            try
            {
                int desiredTickets = GetDesiredTickets(player);

                decimal ticketPrice = settings.Price;
                int affordableTickets = (int)(player.Balance / ticketPrice);
                int ticketsToBuy = Math.Min(desiredTickets, affordableTickets);
                ticketsToBuy = Math.Min(ticketsToBuy, settings.MaximumToPurchase);
                decimal ticketsCost = ticketsToBuy * settings.Price;

                if (ticketsToBuy <= 0)
                {
                    logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.InsufficientFunds, player.Balance, ticketsCost));
                    return;
                }

                player.DescreaseBalance(ticketsCost);
                player.IncreaseTicketsPurchased(ticketsToBuy);

                for (int i = 1; i <= ticketsToBuy; i++)
                {
                    ITicket ticket = ticketFactory.CreateTicket(gameType, player, settings.Price);
                    player.AddTicket(ticket);
                }
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryTicketsService), nameof(ProcessPlayerTickets));
            }
        }

        private int GetDesiredTickets(IPlayer player)
        {
            try
            {
                int minimumToPurchase = settings.MinimumToPurchase;
                int maximumToPurchase = settings.MaximumToPurchase;

                if (player.Name == AppConstants.UserPlayer)
                {
                    LogTicketInformation(player);

                    bool input = int.TryParse(reader.Read(), out int desiredTickets);
                    bool validTicketRange = desiredTickets >= minimumToPurchase && desiredTickets <= maximumToPurchase;

                    if (input && validTicketRange)
                    {
                        return desiredTickets;
                    }

                    logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.InputValidation, minimumToPurchase, maximumToPurchase));
                    return 0;
                }
                else
                {
                    return randomNumberGenerator.Next(minimumToPurchase, maximumToPurchase + 1);
                }
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryTicketsService), nameof(GetDesiredTickets));
                return 0;
            }
        }

        private void LogTicketInformation(IPlayer player)
        {
            try
            {
                logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.TicketPrice, settings.Price));
                logger.Log();
                logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.TicketBuy, player.Name));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryTicketsService), nameof(LogTicketInformation));
            }
        }

        private void LogPurchaseSummary(IGame game)
        {
            try
            {
                bool isUserPlayerInGame = game.Players.Any(p => p.Name == AppConstants.UserPlayer);
                int gamePlayersCount = game.Players.Count;

                logger.Log();
                logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.PurchasedTickets, isUserPlayerInGame ? gamePlayersCount - 1 : gamePlayersCount));
                logger.Log();
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryTicketsService), nameof(LogPurchaseSummary));
            }
        }
    }
}
