using Microsoft.Extensions.Configuration;

namespace VehiTrack
{
    public class AppSettings
    {
        public static string ConnectionString { get; private set; } = string.Empty;
        public static string TelegramBotToken { get; private set; } = string.Empty;

        static AppSettings()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .Build();

            if (config["ConnectionString"] == null)
            {
                throw new Exception("ConnectionString not found in appsettings.json");
            }

            if (config["TelegramBotToken"] == null)
            {
                throw new Exception("TelegramBotToken not found in appsettings.json");
            }

            ConnectionString = config["ConnectionString"] ?? string.Empty;
            TelegramBotToken = config["TelegramBotToken"] ?? string.Empty;
        }
    }
}
