namespace SimplifiedLotteryGame.App.Infrastructure.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Common.Utilities;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Models.Settings;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Domain.Services;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettings(configuration);

            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<IReader, ConsoleReader>();
            services.AddSingleton<ILogger, ConsoleLogger>();

            services.AddSingleton<ITicketFactory, TicketFactory>();
            services.AddSingleton<IPlayerFactory, PlayerFactory>();
            services.AddSingleton<IPrizeFactory, PrizeFactory>();
            services.AddSingleton<IGameFactory, GameFactory>();

            services.AddScoped<IPlayersService, LotteryPlayersService>();
            services.AddScoped<ITicketsService, LotteryTicketsService>();
            services.AddScoped<ILotteryGrandPrizesService, LotteryGrandPrizesService>();
            services.AddScoped<ILotteryTierPrizesService, LotteryTierPrizesService>();
            services.AddScoped<IPrizesService, LotteryPrizesService>();
            services.AddScoped<IGamesService, LotteryGamesService>();

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSettings<GameSettings>(configuration);
            services.ConfigureSettings<PlayersSettings>(configuration);
            services.ConfigureSettings<TicketsSettings>(configuration);
            services.ConfigureSettings<PrizesSettings>(configuration);

            return services;
        }

        private static IServiceCollection ConfigureSettings<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class
        {
            services
                .Configure<T>(configuration.GetSection(typeof(T).Name))
                .PostConfigure<T>(settings =>
                {
                    if (settings == null)
                    {
                        throw new ArgumentNullException(string.Format(AppConstants.Messages.Errors.MissingConfiguration, typeof(T).Name));
                    }
                });

            services.AddSingleton(provider => provider.GetRequiredService<IOptions<T>>().Value);

            return services;
        }
    }
}
