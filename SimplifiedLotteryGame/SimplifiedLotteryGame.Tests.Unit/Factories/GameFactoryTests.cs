namespace SimplifiedLotteryGame.Tests.Unit.Factories
{
    using SimplifiedLotteryGame.Domain.Factories.Contracts;
    using SimplifiedLotteryGame.Domain.Factories;
    using SimplifiedLotteryGame.Domain.Models;
    using SimplifiedLotteryGame.Common;

    using Xunit;

    public class GameFactoryTests
    {
        private const string invalidGameType = "InvalidGame";
        private readonly IGameFactory factory;

        public GameFactoryTests()
        {
            factory = new GameFactory();
        }

        [Fact]
        public void CreateGame_ShouldReturnGameWithProperties()
        {
            LotteryGame game = (LotteryGame)factory.CreateGame(AppConstants.GameTypes.Lottery);

            Assert.NotNull(game);
            Assert.Equal(0, game.TotalTicketRevenue);
            Assert.Equal(0, game.Profit);
            Assert.Empty(game.Players);
            Assert.Empty(game.Prizes);
        }

        [Fact]
        public void CreateGame_ShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => factory.CreateGame(invalidGameType));
        }
    }
}
