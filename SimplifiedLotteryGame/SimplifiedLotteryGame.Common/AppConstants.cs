namespace SimplifiedLotteryGame.Common
{
    public static class AppConstants
    {
        public const string ConfigurationFile = "appsettings.json";
        public const string TestConfigurationFile = "appsettings.Test.json";
        public const string PlayerName = "Player";
        public const string UserPlayer = $"{PlayerName} 1";
        public const int PercentDivider = 100;

        public static class GameTypes
        {
            public const string Lottery = "Lottery";
        }

        public static class PrizeTypes
        {
            public const string LotteryGrand = "LotteryGrand";
            public const string LotterySecondTier = "LotterySecondTier";
            public const string LotteryThirdTier = "LotteryThirdTier";
        }

        public static class Messages
        {
            public const string InsufficientFunds = "Insufficient funds";

            public static class Errors
            {
                public const string General = "Error ({0}) occured in {1}.{2}: {3}";
                public const string MissingConfiguration = "'{0}' configuration is missing in configuration file";
                public const string InvalidGameType = "'{0}' game type not supported";
                public const string InvalidPrizeType = "'{0}' prize type not supported";
                public const string ConfigurationError = "Configuration error: {0}";
                public const string OperationError = "Operation error: {0}";
                public const string ExpectedGame = "Game model '{0}' expected";
                public const string GameTypeNotImplemented = "Game type '{0}' not implemented";
                public const string NullService = "Service '{0}' is null";
                public const string NullParameter = "Parameter '{0}' is null in {1}.{2}";
                public const string InvalidPropertyValue = "Property '{0}' value is invalid in {1}.{2}";
            }

            public static class PrizeDetermination
            {
                public const string Results = "Ticket Draw Results:";
                public const string NoWinningTickets = "No winning tickets for '{0}' prize";
                public const string GrandPrizeWin = "* {0}: {1} wins ${2:F2}";
                public const string TierPrizeWin = "* {0}: Players {1} win ${2:F2} each!";
                public const string Congratulations = "Congratulations to the winners!";
                public const string HouseRevenue = "House Revenue: ${0:F2}";
            }

            public static class PlayerInitialization
            {
                public const string Welcome = "Welcome to Bede Lottery, {0}!";
                public const string Balance = "* Your digital balance: ${0:F2}";
                public const string TicketPrice = "* Ticket Price: ${0:F2} each";
                public const string TicketBuy = "How many tickets do you want to buy, {0}?";
                public const string InputValidation = "Invalid input. Please input number between {0} and {1}";
                public const string FailedConsolePlayerCreation = "Could not create console player";
                public const string PurchasedTickets = "{0} other CPU players also have purchased tickets.";
                public const string InsufficientFunds = "Insufficient funds. Available balance ${0:F2}. Tickets cost ${1:F2}";
            }
        }

        public static class Tests
        {
            public const int DesiredTickets = 5;
            public const int InvalidTicketsAmount = 20;
            public const int RandomNumberGeneratorReturnValue = 5;
            public const int RandomPlayersCount = 13;
            public const int MinimumPlayersToParticipate = 10;
            public const int MaximumPlayersToParticipate = 15;
            public const string UserPlayerName = "Player 1";
            public const string CPUPlayerName = "Player 2";
            public const string InsufficientFunds = "Insufficient funds";
            public const string InvalidInput = "Invalid input";
        }
    }
}
