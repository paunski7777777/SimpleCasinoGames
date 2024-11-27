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

    public class TicketsServiceTests : UnitTestBase
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

        public TicketsServiceTests()
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
        public void PurchaseTickets_ShouldReturnPlayersLotteryTickets()
        {
            IEnumerable<ITicket> playersTickets = lotteryGamesService.Game.Players.SelectMany(p => ((LotteryPlayer)p).Tickets);

            Assert.NotEmpty(playersTickets);
        }

        [Fact]
        public void PurchaseTickets_ShouldReturnPlayersDecreasedBalances()
        {
            IEnumerable<decimal> playersBalances = lotteryGamesService.Game.Players.Select(p => p.Balance);

            Assert.All(playersBalances, balance => Assert.True(balance < 10));
            Assert.All(playersBalances, balance => Assert.True(balance == DesiredTickets));
        }

        [Fact]
        public void PurchaseTickets_ShouldReturnPlayersIncreasedTicketsPurchased()
        {
            IEnumerable<int> playersTicketsPurchased = lotteryGamesService.Game.Players.Select(p => ((LotteryPlayer)p).TicketsPurchased);

            Assert.All(playersTicketsPurchased, tickets => Assert.True(tickets > 0));
            Assert.All(playersTicketsPurchased, tickets => Assert.True(tickets == DesiredTickets));
        }

        [Fact]
        public void PurchaseTickets_ShouldLogPlayersInsufficientFunds()
        {
            loggerMock
                .Setup(l => l.Log(It.IsAny<string>()));

            lotteryGamesService.StartGame();
            lotteryGamesService.StartGame();
            lotteryGamesService.StartGame();

            loggerMock.Verify(logger => logger.Log(It.Is<string>(message => message.Contains(InsufficientFunds))));
        }

        [Fact]
        public void StartGame_PurchaseTickets_ShouldLogUserPlayerInvalidInput()
        {
            readerMock
                .Setup(r => r.Read())
                .Returns(InvalidTicketsAmount.ToString());

            lotteryGamesService.StartGame();

            loggerMock.Verify(logger => logger.Log(It.Is<string>(message => message.Contains(InvalidInput))));
        }
    }
}
