using Flurl.Http;
using RatesApi.DTOs;

namespace RatesApi.Managers
{
    public class RatesManager
    {
        public static async Task<List<RatesDTO>> GetRates()
        {
            var list = await "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json".GetJsonAsync<List<RatesDTO>>();
            return list;
        }
        public static async Task<bool> IsRateExist(string name)
        {
            return (await GetRates()).Count(x => x.Name == name) > 0;
        }
    }
}
