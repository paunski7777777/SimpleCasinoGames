namespace SimplifiedLotteryGame.App
{
    using SimplifiedLotteryGame.Domain.Models.Settings;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Domain.Services;
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.App.Infrastructure.Extensions;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private static void Main()
        {
            try
            {
                IConfiguration configuration = BuildConfiguration();
                ServiceProvider serviceProvider = ConfigureServices(configuration);
                GameSettings? gameSettings = serviceProvider.GetService<GameSettings>() ?? throw new ArgumentException(string.Format(AppConstants.Messages.Errors.MissingConfiguration, nameof(GameSettings)));
                IGamesService gamesService = GetGamesService(serviceProvider, gameSettings.GameType) ?? throw new ArgumentException(string.Format(AppConstants.Messages.Errors.NullService, nameof(IGamesService)));

                gamesService?.StartGame();
                gamesService?.ResetGame();
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(string.Format(AppConstants.Messages.Errors.ConfigurationError, exception.Message));
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine(string.Format(AppConstants.Messages.Errors.OperationError, exception.Message));
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format(AppConstants.Messages.Errors.General, exception.GetType().Name, nameof(Program), nameof(Main), exception.Message));
            }
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppConstants.ConfigurationFile, optional: false, reloadOnChange: true)
                .Build();
        }

        private static ServiceProvider ConfigureServices(IConfiguration configuration)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddApplicationServices(configuration);

            return services.BuildServiceProvider();
        }

        private static IGamesService? GetGamesService(IServiceProvider serviceProvider, string? gameType)
        {
            return gameType switch
            {
                AppConstants.GameTypes.Lottery => serviceProvider.GetService<IGamesService>() as LotteryGamesService,
                _ => throw new NotImplementedException(string.Format(AppConstants.Messages.Errors.GameTypeNotImplemented, gameType))
            };
        }
    }
}