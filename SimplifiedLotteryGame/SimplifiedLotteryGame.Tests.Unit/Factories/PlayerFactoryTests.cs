namespace SimplifiedLotteryGame.Tests.Unit.Factories
{
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Models.Contracts;
    using SimplifiedLotteryGame.Common;
    using SimplifiedLotteryGame.Domain.Models;

    using Xunit;

    public class PlayerFactoryTests
    {
        private const string gameType = AppConstants.GameTypes.Lottery;
        private const string playerName = AppConstants.UserPlayer;
        private const decimal balance = 10;
        private const int negativeBalance = -1;
        private const string invalidGameType = "InvalidGame";
        private readonly IPlayerFactory factory;

        public PlayerFactoryTests()
        {
            factory = new PlayerFactory();
        }

        [Fact]
        public void CreatePlayer_ShouldReturnPlayerWithProperties()
        {
            IPlayer player = factory.CreatePlayer(gameType, playerName, balance);

            Assert.NotNull(player);
            Assert.True(player is LotteryPlayer);
            Assert.Equal(0, ((LotteryPlayer)player).TicketsPurchased);
            Assert.Equal(balance, player.Balance);
            Assert.Equal(AppConstants.UserPlayer, player.Name);
            Assert.Empty(((LotteryPlayer)player).Tickets);
        }

        [Fact]
        public void CreatePlayer_ShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => factory.CreatePlayer(invalidGameType, playerName, balance));
        }

        [Fact]
        public void CreatePlayer_ShouldThrowArgumentNullExceptionForBalance()
        {
            Assert.Throws<ArgumentNullException>(() => factory.CreatePlayer(gameType, playerName, negativeBalance));
        }

        [Fact]
        public void CreatePlayer_ShouldThrowArgumentNullExceptionForName()
        {
            Assert.Throws<ArgumentNullException>(() => factory.CreatePlayer(gameType, null, negativeBalance));
        }
    }
}
