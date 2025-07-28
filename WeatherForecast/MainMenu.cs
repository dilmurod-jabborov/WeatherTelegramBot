using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherForecast.Data.Repository;
using WeatherForecast.Models;

namespace WeatherForecast;

public class MainMenu
{
    private readonly IDictionary<long, UserSession> sessions;
    private readonly UserService userService;
    private readonly ITelegramBotClient botClient;

    public MainMenu(string token)
    {
        sessions = new Dictionary<long, UserSession>();
        botClient = new TelegramBotClient(token);
        userService = new UserService();
    }

    public async Task StartAsync()
    {
        using var cts = new CancellationTokenSource();

        botClient.StartReceiving(
            updateHandler: ShowMainMenu,
            HandleErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.CallbackQuery, UpdateType.Message }
            },
            cancellationToken: cts.Token);

        var me = await botClient.GetMe();
        Console.WriteLine($"✅ Admin bot ishga tushdi: @{me.Username}");

        await Task.Delay(-1);
    }

    private async Task ShowMainMenu(ITelegramBotClient botClient, Update update, CancellationToken ct)
   {
        var chatId = update.Message?.Chat.Id
                     ?? update.CallbackQuery?.Message.Chat.Id
                     ?? 0;

        await CallBackQueryHelper(chatId, update, ct);

        await MessageHelper(chatId, update, ct);
    }

    public async Task ShowMenuAsync(long chatId)
    {

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
{
        new[]
        {
         InlineKeyboardButton.WithCallbackData("🌞 Kunlik", "weather_daily"),
         InlineKeyboardButton.WithCallbackData("📆 Haftalik", "weather_weekly"),
         InlineKeyboardButton.WithCallbackData("📅 Oylik", "weather_monthly")
        }
        });

        await botClient.SendMessage(
            chatId: chatId,
            text: "Qaysi davr uchun ob-havo ma'lumotini ko‘rmoqchisiz?",
            replyMarkup: inlineKeyboard
        );
    }

    private async Task CallBackQueryHelper(long chatId, Update update, CancellationToken ct)
    {
        if (update.CallbackQuery != null)
        {
            var data = update.CallbackQuery.Data;

            if (data == "weather_daily" || data == "weather_weekly" || data == "weather_monthly")
            {
                if (!sessions.TryGetValue(chatId, out var session))
                {
                    session = new UserSession();
                    sessions[chatId] = session;
                }

                session.Data["SelectedPeriod"] = data;

                if (!session.Data.ContainsKey("IsRegistered") || session.Data["IsRegistered"] != "true")
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                    new[] { InlineKeyboardButton.WithCallbackData("🔐 Ro'yxatdan o'tish", "register_start") }
                    });

                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "Iltimos, avval ro'yxatdan o'ting!",
                        replyMarkup: inlineKeyboard
                    );

                    return;
                }

                var requestLocationKeyboard = new ReplyKeyboardMarkup(
    new[]
    {
        new KeyboardButton[]
        {
            KeyboardButton.WithRequestLocation("📍 Lokatsiyani yuborish")
        }
    })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                await botClient.SendMessage(
                    chatId,
                    "📍 Iltimos, lokatsiyangizni yuboring.",
                    replyMarkup: requestLocationKeyboard
                );

            }
            else if (data == "register_start")
            {
                sessions[chatId] = new UserSession
                {
                    Mode = "register",
                    CurrentStep = "firstname"
                };

                await botClient.SendMessage(chatId, "👤 Enter First name...", cancellationToken: ct);
                return;
            }
        }
    }

    private async Task MessageHelper(long chatId, Update update, CancellationToken ct)
    {
        try
        {
            if (update.Message?.Location != null)
            {
                var location = update.Message.Location;
                if (sessions.TryGetValue(chatId, out var session) && session.Data.TryGetValue("SelectedPeriod", out var period))
                {
                    var weatherService = new WeatherService();
                    WeatherModel? weather = null;

                    if (period == "weather_daily")
                        weather = await weatherService.GetDailyWeatherAsync(location.Latitude, location.Longitude);
                    else if (period == "weather_weekly")
                        weather = await weatherService.GetWeeklyWeatherAsync(location.Latitude, location.Longitude);
                    else if (period == "weather_monthly")
                        weather = await weatherService.GetMonthlyWeatherAsync(location.Latitude, location.Longitude);

                    if (weather is not null)
                    {
                        string message = BuildWeatherMessage(weather);
                        await botClient.SendMessage(chatId, message);
                    }
                    else
                    {
                        await botClient.SendMessage(chatId, "❌ Ob-havo ma'lumotlarini olishning imkoni bo‘lmadi.");
                    }
                }

                return;
            }

            if (update.Message?.Text is string userInput)
            {
                if (userInput == "/start")
                {
                    await ShowMenuAsync(chatId);
                    return;
                }
                if (sessions.TryGetValue(chatId, out var session))
                {
                    if (session.Mode == "register")
                        await Register(chatId, userInput, update, session, ct);
                }
            }
            if (update.Message?.Contact is { } contact)
            {
                if (sessions.TryGetValue(chatId, out var session) &&
                    session.Mode == "register" &&
                    session.CurrentStep == "phone")
                {
                    session.Data["phone"] = NormalizerPhone(contact.PhoneNumber);

                    try
                    {
                        userService.Register(session.Data["firstname"], session.Data["lastname"], session.Data["phone"]);

                        await botClient.SendMessage(chatId, "✅ Registered!", cancellationToken: ct);
                        await botClient.SendMessage(chatId, "⬅️ To return to the main menu, type /start!", cancellationToken: ct);
                    }
                    catch (Exception ex)
                    {
                        await botClient.SendMessage(chatId, $"❌ Error: {ex.Message}", cancellationToken: ct);
                    }
                    session.Data["IsRegistered"] = "true";
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await botClient.SendMessage(chatId, $"Error: {ex.Message}", cancellationToken: ct);
        }
    }

    private string BuildWeatherMessage(WeatherModel model)
    {
        var sb = new StringBuilder();
        sb.AppendLine("🌤 Ob-havo ma'lumotlari:");
        sb.AppendLine();

        for (int i = 0; i < model.Daily.Time.Count; i++)
        {
            sb.AppendLine($"📅 Sana: {model.Daily.Time[i]}");
            sb.AppendLine($"🌡 Minimal: {model.Daily.Temperature_2m_Min[i]}°C");
            sb.AppendLine($"🌡 Maksimal: {model.Daily.Temperature_2m_Max[i]}°C");
            sb.AppendLine($"🌧 Yomg'ir: {model.Daily.Precipitation_Sum[i]} mm");
            sb.AppendLine($"🌬 Shamol: {model.Daily.Wind_Speed_10m_Max[i]} km/h");
            sb.AppendLine("➖➖➖➖➖➖➖➖");
        }

        return sb.ToString();
    }

    private string NormalizerPhone(string phone)
    {
        return phone.Replace("+", "").Trim();
    }

    private Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
        Console.WriteLine($"❌ Xatolik: {exception.Message}");
        return Task.CompletedTask;
    }

    private async Task Register(long chatId, string userInput, Update update, UserSession session, CancellationToken ct)
    {
        if (userInput == "/start")
        {
            await ShowMenuAsync(chatId);
            sessions.Remove(chatId);
            return;
        }

        switch (session.CurrentStep)
        {
            case "firstname":
                session.Data["firstname"] = userInput;
                session.CurrentStep = "lastname";
                await botClient.SendMessage(chatId, "👤 Enter last name...", cancellationToken: ct);
                break;

            case "lastname":
                session.Data["lastname"] = userInput;
                session.CurrentStep = "phone";

                var contactKeyboard =
                                new ReplyKeyboardMarkup(new[]
                                {
                        KeyboardButton.WithRequestContact("📱 Send your phone number")
                                })
                                {
                                    ResizeKeyboard = true,
                                    OneTimeKeyboard = true
                                };

                await botClient.SendMessage(chatId, "📞 Click button to send phone number..",
                    replyMarkup: contactKeyboard, cancellationToken: ct);
                break;
        }
    }
}
