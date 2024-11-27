namespace SimplifiedLotteryGame.Domain.Models.Contracts
{
    public interface IPlayer
    {
        string Name { get; }
        decimal Balance { get;  }
    }
}
