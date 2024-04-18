using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using VehiTrack.Services;

namespace VehiTrack
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _client;
        private readonly UserService _userService;

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
            // Only process message updates
            if (update.Message is not { } message)
                return;

            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            // Only process real users
            if (message.From is null || message.From.IsBot)
                return;

            var context = new TelegramBotContext()
            {
                BotClient = botClient,
                Message = message,
                CancellationToken = cancellationToken,
                User = await GetUserAsync(message.From)
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
                    await HandleListRefuelingRecordsCommandAsync(context);
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
                chatId: ctx.Message.Chat.Id,
                text: stringBuilder.ToString(),
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleHelpCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: GetHelpMessage(),
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleAddVehicleCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
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
                chatId: ctx.Message.Chat.Id,
                text: stringBuilder.ToString(),
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleRemoveVehicleCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleUpdateVehicleCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleAddRefuelingRecordCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleListRefuelingRecordsCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleRemoveRefuelingRecordCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }

        private static async Task HandleUpdateRefuelingRecordCommandAsync(TelegramBotContext ctx)
        {
            await ctx.BotClient.SendTextMessageAsync(
                chatId: ctx.Message.Chat.Id,
                text: "⚠️ Em construção...",
                cancellationToken: ctx.CancellationToken
            );
        }
    }

    public struct TelegramBotContext
    {
        public required ITelegramBotClient BotClient { get; set; }
        public required Message Message { get; set; }
        public required CancellationToken CancellationToken { get; set; }
        public required Models.User User { get; set; }
    }
}
