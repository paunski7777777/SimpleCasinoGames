namespace SimplifiedLotteryGame.Tests.Unit.Factories
{
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Common;

    using Xunit;

    public class TicketFactoryTests
    {
        private const string gameType = AppConstants.GameTypes.Lottery;
        private const string playerName = AppConstants.UserPlayer;
        private const decimal balance = 10;
        private const decimal price = 1;
        private const string invalidGameType = "InvalidGame";
        private readonly IPlayerFactory playerFactory;
        private readonly ITicketFactory ticketFactory;

        public TicketFactoryTests()
        {
            ticketFactory = new TicketFactory();
            playerFactory = new PlayerFactory();
        }

        [Fact]
        public void CreateTicket_ShouldReturnTicketWithProperties()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, playerName, balance);
            ITicket ticket = ticketFactory.CreateTicket(gameType, player, price);

            Assert.NotNull(ticket);

            Assert.Equal(price, ticket.Price);
            Assert.Null(ticket.Prize);

            Assert.NotEqual(Guid.Empty, ticket.Id);
        }

        [Fact]
        public void CreateTicket_ShouldThrowInvalidOperationException()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, playerName, balance);

            Assert.Throws<InvalidOperationException>(() => ticketFactory.CreateTicket(invalidGameType, player, price));
        }

        [Fact]
        public void CreatePlayer_ShouldThrowArgumentNullExceptionForPrice()
        {
            IPlayer player = playerFactory.CreatePlayer(gameType, playerName, balance);

            Assert.Throws<ArgumentNullException>(() => ticketFactory.CreateTicket(gameType, player, 0));
        }

        [Fact]
        public void CreatePlayer_ShouldThrowArgumentNullExceptionForPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => ticketFactory.CreateTicket(gameType, null, price));
        }
    }
}
