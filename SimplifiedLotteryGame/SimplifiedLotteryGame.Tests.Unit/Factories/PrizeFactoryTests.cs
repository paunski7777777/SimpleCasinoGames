namespace SimplifiedLotteryGame.Tests.Unit.Factories
{
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Domain.Models.Contracts;

    using Xunit;

    public class PrizeFactoryTests
    {
        private const string gameType = AppConstants.GameTypes.Lottery;
        private const decimal playerBalance = 10;
        private const decimal prizeAmount = 20;
        private const decimal ticketPrice = 1;
        private const string invalidGameType = "InvalidGame";
        private readonly IPrizeFactory prizeFactory;
        private readonly IPlayerFactory playerFactory;
        private readonly ITicketFactory ticketFactory;

        public PrizeFactoryTests()
        {
            prizeFactory = new PrizeFactory();
            ticketFactory = new TicketFactory();
            playerFactory = new PlayerFactory();
        }

        [Fact]
        public void CreatePrize_ShouldReturnGrandPrize()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, AppConstants.UserPlayer, playerBalance);
            ITicket ticket = ticketFactory.CreateTicket(gameType, player, ticketPrice);
            IPrize prize = prizeFactory.CreatePrize(AppConstants.PrizeTypes.LotteryGrand, prizeAmount, ticket);

            Assert.NotNull(prize);
            Assert.Equal(prizeAmount, prize.Amount);
            Assert.True(prize is LotteryGrandPrize);
            Assert.NotNull(((LotteryGrandPrize)prize).Ticket);
        }

        [Fact]
        public void CreatePrize_ShouldReturnSecondTierPrize()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, AppConstants.UserPlayer, playerBalance);
            ITicket ticket = ticketFactory.CreateTicket(gameType, player, ticketPrice);
            IPrize prize = prizeFactory.CreatePrize(AppConstants.PrizeTypes.LotterySecondTier, prizeAmount, ticket);

            Assert.NotNull(prize);
            Assert.Equal(prizeAmount, prize.Amount);
            Assert.True(prize is LotterySecondTierPrize);
            Assert.NotNull(((LotterySecondTierPrize)prize).Ticket);
        }

        [Fact]
        public void CreatePrize_ShouldReturnThirdTierPrize()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, AppConstants.UserPlayer, playerBalance);
            ITicket ticket = ticketFactory.CreateTicket(gameType, player, ticketPrice);
            IPrize prize = prizeFactory.CreatePrize(AppConstants.PrizeTypes.LotteryThirdTier, prizeAmount, ticket);

            Assert.NotNull(prize);
            Assert.Equal(prizeAmount, prize.Amount);
            Assert.True(prize is LotteryThirdTierPrize);
            Assert.NotNull(((LotteryThirdTierPrize)prize).Ticket);
        }

        [Fact]
        public void CreatePrize_ShouldThrowInvalidOperationException()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, AppConstants.UserPlayer, playerBalance);
            ITicket ticket = ticketFactory.CreateTicket(gameType, player, ticketPrice);

            Assert.Throws<InvalidOperationException>(() => prizeFactory.CreatePrize(invalidGameType, prizeAmount, ticket));
        }

        [Fact]
        public void CreatePlayer_ShouldThrowArgumentNullExceptionForPrizeAmount()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, AppConstants.UserPlayer, playerBalance);
            ITicket ticket = ticketFactory.CreateTicket(gameType, player, ticketPrice);

            Assert.Throws<ArgumentNullException>(() => prizeFactory.CreatePrize(gameType, 0, ticket));
        }

        [Fact]
        public void CreatePlayer_ShouldThrowArgumentNullExceptionForTicket()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, AppConstants.UserPlayer, playerBalance);

            Assert.Throws<ArgumentNullException>(() => prizeFactory.CreatePrize(gameType, prizeAmount, null));
        }
    }
}
