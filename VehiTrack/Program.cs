using VehiTrack.Models;
using VehiTrack.Services;

namespace VehiTrack
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // var bot = new TelegramBot();
            // await bot.StartAsync();
            var services = new RefuelingRecordService();
            await services.GetStatisticalDataOnConsumptionAndSupplyCosts(1);
        }
    }
}
