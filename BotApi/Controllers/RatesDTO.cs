
namespace BotApi.Controllers
{
    public class RatesDTO
    {
        public string Name { get; set; }

        public double Rate { get; set; }
        public DateTime ExchangeRate { get; set; }
        public override string ToString()
        {
            return $"Name = {Name} Rate = {Rate} Exchange Date = {ExchangeRate}";
        }
    }
}
