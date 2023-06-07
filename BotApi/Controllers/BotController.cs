using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScottPlot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new[]
               {
                    new []
                    {
                        new KeyboardButton("Додати валюту на сповіщення"),
                    },
                    new[]
                    {
                        new KeyboardButton("Переглянути інформацію про валюту"),
                    }
                })
        {
            ResizeKeyboard = true
        };

        private TelegramBotClient bot = new TelegramBotClient("6192636434:AAEbnjHeE5AG7CAderwuA3sodxSSAVuKXxo");
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                string text = "answer";
                if (update.Message.Text == "/start")
                {
                    text = "Бот виводить дані про зміну ціни певної валюти, її графік та сповіщає користувача.";
                    await bot.SendTextMessageAsync(update.Message.From.Id, text, replyMarkup: replyKeyboard);
                }
                else if (update.Message.Text == "Додати валюту на сповіщення")
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, "Введіть назву валюти:\n\nНаприклад: USD", replyMarkup: replyKeyboard);

                }
                else if (update.Message.Text == "Переглянути інформацію про валюту")
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, "Введіть назву валюти:\n\nНаприклад: USD-info", replyMarkup: replyKeyboard);
                }
                else if (update.Message.Text.Contains("-info"))
                {
                    try
                    {
                        var currency = update.Message.Text.Split("-")[0];
                        var data = await $"https://localhost:7166/api/Rates/{currency}".GetJsonAsync<RatesDTO>();
                        await bot.SendTextMessageAsync(update.Message.From.Id, data.ToString(), replyMarkup: new InlineKeyboardMarkup(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("Отримати графік " + currency),
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        await bot.SendTextMessageAsync(update.Message.From.Id, "Таку валюту не знайдено");
                    }
                   
                }
                else
                {
                    var list = await $"https://localhost:7166/api/Rates".GetJsonAsync<List<RatesDTO>>();
                    if(list.FirstOrDefault(x => x.Name == update.Message.Text) != null)
                    {
                        if(System.IO.File.Exists(Directory.GetCurrentDirectory() + $"/users/{update.Message.From.Id}.json"))
                        {
                            var data = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + $"/users/{update.Message.From.Id}.json");
                            var currencies = JsonConvert.DeserializeObject<List<string>>(data);
                            currencies.Add(update.Message.Text);
                            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + $"/users/{update.Message.From.Id}.json", JsonConvert.SerializeObject(currencies));
                        }
                        else
                        {
                            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + $"/users/{update.Message.From.Id}.json", JsonConvert.SerializeObject(new List<string> { update.Message.Text}));
                        }
                        await bot.SendTextMessageAsync(update.Message.From.Id, "Успішно підписано на оновлення по валюті");

                    }
                    else
                    {
                        await bot.SendTextMessageAsync(update.Message.From.Id, "Таку валюту не знайдено");
                    }
                }

            }
            else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var str = update.CallbackQuery.Data;
                if(str.Contains("Отримати графік"))
                {
                    var temp = str.Split(" ");
                    var currency = temp[temp.Length - 1];
                    var list = await $"https://localhost:7166/api/Rates".GetJsonAsync<List<RatesDTO>>();
                    list = list.Where(x => x.Name == currency).ToList();

                    double[] positions = DataGen.Consecutive(list.Count);

                    var plt = new ScottPlot.Plot(400, 300);
                    plt.AddScatter(positions, list.Select(x => x.Rate).ToArray());
                    plt.SaveFig("quickstart.png");

                    using (Stream stream = System.IO.File.OpenRead("quickstart.png"))
                    {
                        // Створюємо об'єкт InputOnlineFile з потоком зображення
                        var inputFile = InputFile.FromStream(stream);

                        // Надсилаємо зображення до бота
                        Message message = await bot.SendPhotoAsync(update.CallbackQuery.From.Id, inputFile);

                        // Перевіряємо, чи надіслано повідомлення успішно
                        if (message != null)
                        {
                            Console.WriteLine("Image sent successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to send the image.");
                        }
                    }
                }
            }
            return Ok();
        }
        [HttpGet]
        public string Get()
        {
            return "Telegram bot was started";
        }
    }
}
