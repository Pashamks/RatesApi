using BotApi.Controllers;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Threading;
using Telegram.Bot;

public class TimerService
{
    private Timer _timer;
    TelegramBotClient bot = new TelegramBotClient("6192636434:AAEbnjHeE5AG7CAderwuA3sodxSSAVuKXxo");
    public void StartTimer()
    {
        // Create a timer that executes the TimerCallback method every 1 second
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    }

    private void TimerCallback(object state)
    {
        string[] fileNames = Directory.GetFiles(@"C:\Users\pavlo\Desktop\bot\apiandbot\RatesApi\BotApi\users");
        var dic = new Dictionary<string, List<string>>();

        foreach (string fileName in fileNames)
        {
            var text = File.ReadAllText(fileName);
            var list = JsonConvert.DeserializeObject<List<string>>(text);
            dic.Add(fileName.Replace(".json", ""), list);
        }
        var rates =  $"https://localhost:7166/api/Rates".GetJsonAsync<List<RatesDTO>>().Result;

        var groups = rates.GroupBy(x => x.Name);
        foreach (var group in groups)
        {
            if(group.Count() >= 2)
            {
                var sorted = group.OrderBy(x => x.ExchangeRate).ToList();
                if (sorted[0].Rate - sorted[1].Rate > sorted[0].Rate * 0.02)
                {
                    foreach (var item in dic)
                    {
                        if (item.Value.Contains(sorted[0].Name))
                        {
                            var temp = item.Key.Split("\\");
                            bot.SendTextMessageAsync(temp[temp.Length-1], $"Ціна валюти {sorted[0].Name} змінилася більше ніж на 2%").Wait();
                        }
                    }
                }
            }
        }
    }

    public void StopTimer()
    {
        // Stop the timer
        _timer?.Dispose();
    }
}
