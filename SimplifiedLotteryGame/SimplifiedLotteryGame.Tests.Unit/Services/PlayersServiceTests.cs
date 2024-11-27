namespace SimplifiedLotteryGame.Tests.Unit.Services
{
    using Moq;

    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Domain.Models.Settings;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Domain.Services;
    using SimplifiedLotteryGame.Domain.Models;

    using Xunit;

    using static SimplifiedLotteryGame.Common.AppConstants.Tests;

    public class PlayersServiceTests : UnitTestBase
    {
        private readonly Mock<IRandomNumberGenerator> randomNumberGeneratorMock;
        private readonly Mock<IReader> readerMock;
        private readonly Mock<ILogger> loggerMock;
        private readonly IPlayerFactory playerFactory;
        private readonly IPlayersService playersService;

        public PlayersServiceTests()
        {
            PlayersSettings playersSettings = PlayersSettingsOptions.Value;
            TicketsSettings ticketsSettings = TicketsSettingsOptions.Value;
            PrizesSettings prizesSettings = PrizesSettingsOptions.Value;

            randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();
            readerMock = new Mock<IReader>();
            loggerMock = new Mock<ILogger>();
            playerFactory = new PlayerFactory();

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
        }

        [Fact]
        public void InitializePlayers_CreateUserPlayer_ShouldReturnGameUserPlayer()
        {
            Mock<LotteryGame> game = new();

            playersService.InitializePlayers(game.Object);

            Assert.Contains(game.Object.Players, a => a.Name == UserPlayerName);
        }

        [Fact]
        public void InitializePlayers_AddCPUPlayers_ShouldReturnCPUPlayers()
        {
            randomNumberGeneratorMock
                .Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(RandomPlayersCount);

            Mock<LotteryGame> game = new();

            playersService.InitializePlayers(game.Object);

            IEnumerable<IPlayer> gamePlayers = game.Object.Players;
            int gamePlayersCount = gamePlayers.Count();

            Assert.NotNull(gamePlayers);
            Assert.NotEmpty(gamePlayers);
            Assert.True(gamePlayersCount > 1);
            Assert.True(gamePlayersCount == RandomPlayersCount);
            Assert.InRange(gamePlayersCount, MinimumPlayersToParticipate, MaximumPlayersToParticipate);
            Assert.Contains(game.Object.Players, a => a.Name == CPUPlayerName);
        }
    }
}
