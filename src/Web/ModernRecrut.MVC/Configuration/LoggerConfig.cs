namespace ModernRecrut.MVC.Configuration
{
    public static class LoggerConfig
    {
        public static void AddLoggerConfiguration(this ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.AddEventLog();
        }
    }
}
