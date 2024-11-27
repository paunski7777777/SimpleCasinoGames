namespace SimplifiedLotteryGame.Domain.Models.Contracts
{
    public interface IGame
    {
        decimal Profit { get;  }
        bool ArePlayersInitialized { get;  }
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<IPrize> Prizes { get; }
    }
}
