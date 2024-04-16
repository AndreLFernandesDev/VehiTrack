using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace VehiTrack
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _client;

        public TelegramBot()
        {
            _client = new TelegramBotClient(AppSettings.TelegramBotToken);
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

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Você disse:\n" + messageText,
                cancellationToken: cancellationToken
            );
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
    }
}
