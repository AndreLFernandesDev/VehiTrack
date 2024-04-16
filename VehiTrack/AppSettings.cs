using Microsoft.Extensions.Configuration;

namespace VehiTrack
{
    public static class AppSettings
    {
        public static string ConnectionString { get; private set; } = string.Empty;
        public static string TelegramBotToken { get; private set; } = string.Empty;

        static AppSettings()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .Build();

            if (config["ConnectionString"] is not { } connectionString)
            {
                throw new Exception("ConnectionString not found in appsettings.json");
            }

            if (config["TelegramBotToken"] is not { } telegramBotToken)
            {
                throw new Exception("TelegramBotToken not found in appsettings.json");
            }

            ConnectionString = connectionString;
            TelegramBotToken = telegramBotToken;
        }
    }
}
