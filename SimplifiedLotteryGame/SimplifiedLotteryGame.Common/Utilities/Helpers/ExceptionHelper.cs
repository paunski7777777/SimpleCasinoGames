namespace SimplifiedLotteryGame.Common.Utilities.Helpers
{
    using SimplifiedLotteryGame.Common.Utilities.Contracts;

    public static class ExceptionHelper
    {

        public static void HandleInvalidOperationException(ILogger logger, Exception exception, string serviceName, string methodName)
        {
            string errorMessage = FormatErrorMessage(exception, serviceName, methodName);
            logger.Log(errorMessage);
            throw new InvalidOperationException(errorMessage, exception);
        }

        public static void HandleArgumentNullException(ILogger logger, ArgumentNullException exception, string serviceName, string methodName)
        {
            string errorMessage = FormatErrorMessage(exception, serviceName, methodName);
            logger.Log(errorMessage);
            throw new ArgumentNullException(errorMessage, exception);
        }

        private static string FormatErrorMessage(Exception exception, string serviceName, string methodName)
        {
            return string.Format(AppConstants.Messages.Errors.General, exception.GetType().Name, serviceName, methodName, exception.Message);
        }
    }
}
