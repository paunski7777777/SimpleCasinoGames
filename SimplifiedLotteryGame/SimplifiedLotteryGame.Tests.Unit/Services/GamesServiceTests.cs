namespace SimplifiedLotteryGame.Tests.Unit.Services
{
    using Moq;

    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models.Settings;
    using SimplifiedLotteryGame.Domain.Services;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Tests.Unit;

    using Xunit;

    using static SimplifiedLotteryGame.Common.AppConstants.Tests;

    public class GamesServiceTests : UnitTestBase
    {
        private readonly Mock<IRandomNumberGenerator> randomNumberGeneratorMock;
        private readonly Mock<IReader> readerMock;
        private readonly Mock<ILogger> loggerMock;
        private readonly IPlayerFactory playerFactory;
        private readonly ITicketFactory ticketFactory;
        private readonly IPrizeFactory prizeFactory;
        private readonly IGameFactory gameFactory;
        private readonly IPlayersService playersService;
        private readonly ITicketsService ticketsService;
        private readonly IPrizesService prizesService;
        private readonly ILotteryGrandPrizesService grandPrizesService;
        private readonly ILotteryTierPrizesService tierPrizesService;
        private readonly LotteryGamesService lotteryGamesService;

        public GamesServiceTests()
        {
            PlayersSettings playersSettings = PlayersSettingsOptions.Value;
            TicketsSettings ticketsSettings = TicketsSettingsOptions.Value;
            PrizesSettings prizesSettings = PrizesSettingsOptions.Value;

            randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();
            readerMock = new Mock<IReader>();
            loggerMock = new Mock<ILogger>();
            playerFactory = new PlayerFactory();
            ticketFactory = new TicketFactory();
            prizeFactory = new PrizeFactory();
            gameFactory = new GameFactory();

            readerMock
                .Setup(r => r.Read())
                .Returns(DesiredTickets.ToString());

            randomNumberGeneratorMock
                .Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(RandomNumberGeneratorReturnValue);

            playersService = new LotteryPlayersService(
                playersSettings,
                randomNumberGeneratorMock.Object,
                loggerMock.Object,
                playerFactory);

            ticketsService = new LotteryTicketsService(
                ticketsSettings,
                randomNumberGeneratorMock.Object,
                readerMock.Object,
                loggerMock.Object,
                ticketFactory);

            grandPrizesService = new LotteryGrandPrizesService(randomNumberGeneratorMock.Object, loggerMock.Object, prizeFactory);
            tierPrizesService = new LotteryTierPrizesService(randomNumberGeneratorMock.Object, loggerMock.Object, prizeFactory);

            prizesService = new LotteryPrizesService(
                prizesSettings,
                randomNumberGeneratorMock.Object,
                loggerMock.Object,
                prizeFactory,
                grandPrizesService,
                tierPrizesService);

            lotteryGamesService = new LotteryGamesService(
                loggerMock.Object,
                gameFactory,
                playersService,
                ticketsService,
                prizesService);

            lotteryGamesService.StartGame();
        }

        [Fact]
        public void StartGame_ShouldReturnGameWithUpdatedValues()
        {
            Assert.True(lotteryGamesService.Game.ArePlayersInitialized);
            Assert.NotEmpty(lotteryGamesService.Game.Players);
            Assert.NotEmpty(lotteryGamesService.Game.Prizes);
            Assert.True(lotteryGamesService.Game.TotalTicketRevenue > 0);
            Assert.True(lotteryGamesService.Game.Profit > 0);
        }

        [Fact]
        public void ResetGame_ShouldReturnGameWithDefaultValues()
        {
            lotteryGamesService.ResetGame();

            Assert.False(lotteryGamesService.Game.ArePlayersInitialized);
            Assert.Empty(lotteryGamesService.Game.Players);
            Assert.Empty(lotteryGamesService.Game.Prizes);
            Assert.Equal(0, lotteryGamesService.Game.TotalTicketRevenue);
            Assert.Equal(0, lotteryGamesService.Game.Profit);
        }
    }
}
