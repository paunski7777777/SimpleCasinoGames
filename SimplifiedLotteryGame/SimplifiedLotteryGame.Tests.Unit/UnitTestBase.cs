namespace SimplifiedLotteryGame.Tests.Unit
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Models.Settings;

    public class UnitTestBase
    {
        protected IOptions<PlayersSettings> PlayersSettingsOptions { get; private set; }
        protected IOptions<TicketsSettings> TicketsSettingsOptions { get; private set; }
        protected IOptions<PrizesSettings> PrizesSettingsOptions { get; private set; }

        public UnitTestBase()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppConstants.TestConfigurationFile, optional: false, reloadOnChange: true)
                .Build();

            PlayersSettingsOptions = LoadSettings<PlayersSettings>(configuration);
            TicketsSettingsOptions = LoadSettings<TicketsSettings>(configuration);
            PrizesSettingsOptions = LoadSettings<PrizesSettings>(configuration);
        }

        private static IOptions<T> LoadSettings<T>(IConfiguration configuration)
            where T : class, new()
        {
            T settings = configuration
                .GetSection(typeof(T).Name)
                .Get<T>() ?? new T();

            return Options.Create(settings);
        }
    }
}
