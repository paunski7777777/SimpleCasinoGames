namespace SimplifiedLotteryGame.Domain.Services
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Common.Utilities.Helpers;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Services.Contracts;

    public class LotteryGamesService : IGamesService
    {
        private readonly string gameType = AppConstants.GameTypes.Lottery;
        private readonly ILogger logger;
        private readonly IGameFactory gameFactory;
        private readonly IPlayersService playersService;
        private readonly ITicketsService ticketsService;
        private readonly IPrizesService prizesService;

        public LotteryGamesService(ILogger logger, IGameFactory gameFactory, IPlayersService playersService, ITicketsService ticketsService, IPrizesService prizesService)
        {
            this.logger = logger;
            this.gameFactory = gameFactory;
            this.playersService = playersService;
            this.ticketsService = ticketsService;
            this.prizesService = prizesService;
            Game = InitializeGame();
        }

        public LotteryGame Game { get; }

        public void StartGame()
        {
            try
            {
                if (!Game.ArePlayersInitialized)
                {
                    playersService.InitializePlayers(Game);

                    Game.SetArePlayersInitialized();
                }

                ticketsService.PurchaseTickets(Game);
                prizesService.DeterminePrize(Game);
            }
            catch (ArgumentNullException argumentNullException)
            {
                ExceptionHelper.HandleArgumentNullException(logger, argumentNullException, nameof(LotteryGamesService), nameof(StartGame));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryGamesService), nameof(StartGame));
            }
        }

        public void ResetGame()
        {
            try
            {
                if (Game.ArePlayersInitialized)
                {
                    Game.Reset();
                }
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryGamesService), nameof(ResetGame));
            }
        }

        private LotteryGame InitializeGame()
        {
            try
            {
                return (LotteryGame)gameFactory.CreateGame(gameType);
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryGamesService), nameof(InitializeGame));
                throw;
            }
        }
    }
}
