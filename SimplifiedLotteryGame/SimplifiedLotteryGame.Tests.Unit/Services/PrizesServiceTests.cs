namespace SimplifiedLotteryGame.Tests.Unit.Services
{
    using Moq;

    using SimplifiedLotteryGame.Common.Utilities.Contracts;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Domain.Services.Contracts;
    using SimplifiedLotteryGame.Domain.Services;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Models.Settings;
    using SimplifiedLotteryGame.Domain.Models;

    using Xunit;

    using static SimplifiedLotteryGame.Common.AppConstants.Tests;

    public class PrizesServiceTests : UnitTestBase
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

        public PrizesServiceTests()
        {
            PrizesSettings prizesSettings = PrizesSettingsOptions.Value;
            PlayersSettings playersSettings = PlayersSettingsOptions.Value;
            TicketsSettings ticketsSettings = TicketsSettingsOptions.Value;

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
        public void DeterminePrize_ShouldReturnGamePrizes()
        {
            Assert.NotEmpty(lotteryGamesService.Game.Prizes);
        }

        [Fact]
        public void DeterminePrize_ShouldReturnTotalTicketRevenueGreaterThanProfit()
        {
            Assert.True(lotteryGamesService.Game.TotalTicketRevenue > 0);
            Assert.True(lotteryGamesService.Game.TotalTicketRevenue > lotteryGamesService.Game.Profit);
            Assert.True(lotteryGamesService.Game.TotalTicketRevenue == lotteryGamesService.Game.Players.SelectMany(p => ((LotteryPlayer)p).Tickets).Sum(t => t.Price));
        }

        [Fact]
        public void DeterminePrize_ShouldReturnGeneratedPrizes()
        {
            Assert.Contains(lotteryGamesService.Game.Prizes, p => p is LotteryGrandPrize);
            Assert.Contains(lotteryGamesService.Game.Prizes, p => p is LotterySecondTierPrize);
            Assert.Contains(lotteryGamesService.Game.Prizes, p => p is LotteryThirdTierPrize);
        }

        [Fact]
        public void DeterminePrize_ShouldReturnCalculatedPrizes()
        {
            decimal grand = 50;
            decimal secondTier = 30;
            decimal thirdTier = 10;

            IEnumerable<IPrize> prizes = lotteryGamesService.Game.Prizes;
            IEnumerable<IPrize> secondTierPrizes = prizes
                .Where(p => p is LotterySecondTierPrize);
            IEnumerable<IPrize> thirdTierPrizes = prizes
                .Where(p => p is LotteryThirdTierPrize);

            decimal totalTicketRevenue = lotteryGamesService.Game.TotalTicketRevenue;
            decimal expectedGrandPrize = totalTicketRevenue * grand / 100;
            decimal expectedSecondTierPrize = totalTicketRevenue * secondTier / 100;
            decimal expectedThirdTierPrize = totalTicketRevenue * thirdTier / 100;

            decimal expectedSecondTierIndividualPrize = expectedSecondTierPrize / secondTierPrizes.Count();
            decimal expectedThirdTierIndividualPrize = expectedThirdTierPrize / thirdTierPrizes.Count();

            decimal secondTierPrizeAmount = secondTierPrizes.Sum(p => p.Amount);
            decimal thirdTierPrizeAmount = thirdTierPrizes.Sum(p => p.Amount);

            Assert.Equal(secondTierPrizeAmount, expectedSecondTierPrize);
            Assert.Equal(thirdTierPrizeAmount, expectedThirdTierPrize);
            Assert.All(secondTierPrizes, prize => Assert.Equal(expectedSecondTierIndividualPrize, prize.Amount));
            Assert.All(thirdTierPrizes, prize => Assert.Equal(expectedThirdTierIndividualPrize, prize.Amount));
        }
    }
}
