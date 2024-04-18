namespace VehiTrack
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new TelegramBot();
            await bot.StartAsync();
        }
    }
}
