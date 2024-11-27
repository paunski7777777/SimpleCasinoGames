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

    public class LotteryPlayersService(PlayersSettings settings, IRandomNumberGenerator randomNumberGenerator, ILogger logger, IPlayerFactory playerFactory) : IPlayersService
    {
        private readonly string gameType = AppConstants.GameTypes.Lottery;
        private readonly PlayersSettings settings = settings;
        private readonly ILogger logger = logger;
        private readonly IRandomNumberGenerator randomNumberGenerator = randomNumberGenerator;
        private readonly IPlayerFactory playerFactory = playerFactory;

        public void InitializePlayers(LotteryGame game)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(game, string.Format(AppConstants.Messages.Errors.NullParameter, nameof(game), nameof(LotteryPlayersService), nameof(InitializePlayers)));

                CreateUserPlayer(game);

                int totalPlayers = randomNumberGenerator.Next(settings.MinimumToParticipate, settings.MaximumToParticipate + 1);

                AddCPUPlayers(game, totalPlayers);
            }
            catch (ArgumentNullException argumentNullException)
            {
                ExceptionHelper.HandleArgumentNullException(logger, argumentNullException, nameof(LotteryPlayersService), nameof(InitializePlayers));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPlayersService), nameof(InitializePlayers));
            }
        }

        private void CreateUserPlayer(LotteryGame game)
        {
            try
            {
                string playerName = AppConstants.UserPlayer;

                LogIntro(playerName);

                IPlayer player = playerFactory.CreatePlayer(gameType, playerName, settings.Balance);

                game.AddPlayer(player);
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPlayersService), nameof(CreateUserPlayer));
            }
        }

        private void AddCPUPlayers(LotteryGame game, int totalPlayers)
        {
            try
            {
                for (int i = 2; i <= totalPlayers; i++)
                {
                    string playerName = $"{AppConstants.PlayerName} {i}";

                    IPlayer player = playerFactory.CreatePlayer(gameType, playerName, settings.Balance);

                    game.AddPlayer(player);
                }
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPlayersService), nameof(AddCPUPlayers));
            }
        }

        private void LogIntro(string playerName)
        {
            try
            {
                logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.Welcome, playerName));
                logger.Log();
                logger.Log(string.Format(AppConstants.Messages.PlayerInitialization.Balance, settings.Balance));
            }
            catch (Exception exception)
            {
                ExceptionHelper.HandleInvalidOperationException(logger, exception, nameof(LotteryPlayersService), nameof(LogIntro));
            }
        }
    }
}
