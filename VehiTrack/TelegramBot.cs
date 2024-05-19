using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VehiTrack.Services;

namespace VehiTrack
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _client;
        private readonly UserService _userService;
        private const int PAGE_SIZE = 10;

        public TelegramBot()
        {
            _client = new TelegramBotClient(AppSettings.TelegramBotToken);
            _userService = new UserService();
        }

        public async Task StartAsync()
        {
            using CancellationTokenSource cts = new();

            var receiverOptions = new ReceiverOptions() { AllowedUpdates = [] };

            _client.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await _client.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        private async Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken
        )
        {
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
            var messageFrom = update.Message?.From ?? update.CallbackQuery?.From;
            var messageText = update.CallbackQuery?.Data?.Split("#")[0] ?? update.Message?.Text;
            var messageArgs = update.CallbackQuery?.Data?.Split("#").Skip(1).ToList();

            if (chatId is null || messageFrom is null || messageText is null)
                return;

            var context = new TelegramBotContext()
            {
                BotClient = botClient,
                ChatId = chatId ?? 0,
                CancellationToken = cancellationToken,
                User = await GetUserAsync(messageFrom)
            };

            switch (messageText)
            {
                case "/start":
                    await HandleStartCommandAsync(context);
                    break;
                case "/help":
                    await HandleHelpCommandAsync(context);
                    break;
                case "/add_vehicle":
                    await HandleAddVehicleCommandAsync(context);
                    break;
                case "/list_vehicles":
                    await HandleListVehiclesCommandAsync(context);
                    break;
                case "/remove_vehicle":
                    await HandleRemoveVehicleCommandAsync(context);
                    break;
                case "/update_vehicle":
                    await HandleUpdateVehicleCommandAsync(context);
                    break;
                case "/add_refueling_record":
                    await HandleAddRefuelingRecordCommandAsync(context);
                    break;
                case "/list_refueling_records":
                    await HandleListRefuelingRecordsCommandAsync(context, messageArgs);
                    break;
                case "/remove_refueling_record":
                    await HandleRemoveRefuelingRecordCommandAsync(context);
                    break;
                case "/update_refueling_record":
                    await HandleUpdateRefuelingRecordCommandAsync(context);
                    break;

                // // Defining buttons
                // var urlButton = InlineKeyboardButton.WithCallbackData("Go URL1", "abc");
                // var urlButton2 = InlineKeyboardButton.WithCallbackData("Go URL2", "abc");
                // var urlButton3 = InlineKeyboardButton.WithCallbackData("Go URL3", "abc");
                // var urlButton4 = InlineKeyboardButton.WithCallbackData("Go URL4", "abc");

                // // Rows, every row is InlineKeyboardButton[], You can put multiple buttons!
                // var row1 = new InlineKeyboardButton[] { urlButton };
                // var row2 = new InlineKeyboardButton[] { urlButton2, urlButton3 };
                // var row3 = new InlineKeyboardButton[] { urlButton4 };

                // // Buttons by rows
                // var buttons = new InlineKeyboardButton[][] { row1, row2, row3 };

                // // Keyboard
                // var keyboard = new InlineKeyboardMarkup(buttons);

                // // Send Message
                // await botClient.SendTextMessageAsync(
                //     chatId: message.Chat.Id,
                //     text: "Message",
                //     replyMarkup: keyboard,
                //     parseMode: ParseMode.Html,
                //     cancellationToken: cancellationToken
                // );
            }
        }

        private Task HandlePollingErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            var errorMessage = exception switch
            {
                ApiRequestException ex => $"Telegram API Error:\n[{ex.ErrorCode}]\n{ex.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
        }

        private async Task<Models.User> GetUserAsync(User telegramUser)
        {
            var user = await _userService.GetUserByTelegramIdAsync(telegramUser.Id);

            if (user is not null)
                return user;

            user = new Models.User()
            {
                FirstName = telegramUser.FirstName,
                LastName = telegramUser.LastName,
                Username = telegramUser.Username,
                TelegramId = telegramUser.Id
            };

            user = await _userService.CreateUserAsync(user);

            return user;
        }

        private static string GetHelpMessage()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Comandos disponíveis:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("/help - Listar comandos disponíveis");
            stringBuilder.AppendLine("/add_vehicle - Adicionar veículo");
            stringBuilder.AppendLine("/list_vehicles - Listar veículos");
            stringBuilder.AppendLine("/remove_vehicle - Remover veículo");
            stringBuilder.AppendLine("/update_vehicle - Atualizar veículo");
            stringBuilder.AppendLine("/add_refueling_record- Adicionar abastecimento");
            stringBuilder.AppendLine("/list_refueling_records - Listar abastecimentos");
            stringBuilder.AppendLine("/remove_refueling_record - Remover abastecimento");
            stringBuilder.AppendLine("/update_refueling_record - Atualizar abastecimento");

            return stringBuilder.ToString();
        }

        private async Task HandleStartCommandAsync(TelegramBotContext ctx)
        {
            var me = await _client.GetMeAsync(cancellationToken: ctx.CancellationToken);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Olá! Eu sou o {me.FirstName}.");
            stringBuilder.AppendLine("Como posso te ajudar?");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(GetHelpMessage());

            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: stringBuilder.ToString(),
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleHelpCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: GetHelpMessage(),
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleAddVehicleCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleListVehiclesCommandAsync(TelegramBotContext ctx)
        {
            var stringBuilder = new StringBuilder();

            if (ctx.User.Vehicles.Count == 0)
            {
                stringBuilder.AppendLine("Nenhum veículo cadastrado.");
            }
            else
            {
                stringBuilder.AppendLine("Veículos cadastrados:");
                stringBuilder.AppendLine();

                foreach (var vehicle in ctx.User.Vehicles.OrderBy(v => v.Name))
                {
                    stringBuilder.AppendLine($"🚗 {vehicle.Name}");
                }
            }

            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: stringBuilder.ToString(),
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleRemoveVehicleCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleUpdateVehicleCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleAddRefuelingRecordCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleListRefuelingRecordsCommandAsync(
            TelegramBotContext ctx,
            List<string>? args
        )
        {
            var stringBuilder = new StringBuilder();
            InlineKeyboardMarkup? inlineKeyboard = null;

            if (ctx.User.Vehicles.Count == 0)
            {
                stringBuilder.AppendLine("Nenhum veículo cadastrado.");
            }
            else
            {
                int? vehicleId = (args is not null && args.Count >= 1) ? int.Parse(args[0]) : null;
                int page = (args is not null && args.Count >= 2) ? int.Parse(args[1]) : 1;

                if (vehicleId is null)
                {
                    var inlineKeyboardButtons = new List<InlineKeyboardButton>();

                    foreach (var vehicle in ctx.User.Vehicles.OrderBy(v => v.Name))
                    {
                        stringBuilder.AppendLine("Selecione o veículo");

                        inlineKeyboard = new InlineKeyboardMarkup(
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: vehicle.Name,
                                    callbackData: $"/list_refueling_records#{vehicle.Id}#1"
                                )
                            }
                        );
                    }
                }
                else
                {
                    var refuelingRecordService = new RefuelingRecordService();

                    var vehicle = ctx.User.Vehicles.First(v => v.Id == vehicleId);

                    stringBuilder.AppendLine($"🚗 {vehicle.Name}");
                    stringBuilder.AppendLine();

                    var refuelingRecords =
                        await refuelingRecordService.GetExtendedRefuelingRecordsByVehicleId(
                            vehicle.Id
                        );
                    var refuelingRecordsCount = refuelingRecords.Count;

                    refuelingRecords = refuelingRecords
                        .OrderByDescending(r => r.Date)
                        .Skip((page - 1) * PAGE_SIZE)
                        .Take(PAGE_SIZE)
                        .ToList();

                    foreach (var refuelingRecord in refuelingRecords)
                    {
                        var date = refuelingRecord.Date.ToString("dd/MM/yyyy");
                        var quantity = refuelingRecord.Quantity;
                        var odometerCounter = refuelingRecord.OdometerCounter;
                        var consumption = refuelingRecord.Consumption;
                        var unitPrice = refuelingRecord.UnitPrice.ToString("C2");
                        var totalCost = refuelingRecord.TotalCost.ToString("C2");

                        stringBuilder.AppendLine($"📅 {date}");
                        stringBuilder.AppendLine($"⛽ {quantity} L - {unitPrice} - {totalCost}");
                        stringBuilder.AppendLine($"🚗 {odometerCounter} km - {consumption} km/L");
                        stringBuilder.AppendLine();
                    }

                    if (page * PAGE_SIZE < refuelingRecordsCount)
                    {
                        inlineKeyboard = new InlineKeyboardMarkup(
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Próxima página",
                                    callbackData: $"/list_refueling_records#{vehicle.Id}#{page + 1}"
                                )
                            }
                        );
                    }
                }
            }

            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: stringBuilder.ToString(),
                replyMarkup: inlineKeyboard,
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleRemoveRefuelingRecordCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleUpdateRefuelingRecordCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.ChatId,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }
    }

    public struct TelegramBotContext
    {
        public required ITelegramBotClient BotClient { get; set; }
        public required long ChatId { get; set; }
        public required CancellationToken CancellationToken { get; set; }
        public required Models.User User { get; set; }
    }
}
