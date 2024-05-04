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
            var services = new RefuelingRecordsService();
            var lista = await services.CalculateConsumptionAsync(1);
            for (var item = 0; item < lista.Count; item++)
            {
                Console.WriteLine("{0} {1}", lista[item].Id, lista[item].Consumption);
            }
            Console.WriteLine(lista.Count);
        }
    }
}
