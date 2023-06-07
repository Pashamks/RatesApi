
using DatabaseRepository.Models;

namespace DatabaseRepository.Repository
{
    public class RatesRepository
    {
        private EfDbContext GetContext()
        {
            return new EfDbContext();
        }
        public List<RateModel> GetRates()
        {
            using (var ctx = GetContext())
            {
                return ctx.Rates.ToList();
            }
        }
        public void AddRate(RateModel newRate)
        {
            using (var ctx = GetContext())
            {
                ctx.Rates.Add(newRate);
                ctx.SaveChanges();
            }
        }
        public void UpdateRate(RateModel newRate, RateModel oldRate)
        {
            using (var ctx = GetContext())
            {
                var rate = ctx.Rates.First(x => x.Name == oldRate.Name);
                rate.Name = newRate.Name;
                rate.Rate = newRate.Rate;
                rate.ExchangeRate = newRate.ExchangeRate;
                ctx.SaveChanges();
            }
        }
        public void DeleteRate(string name)
        {
            using (var ctx = GetContext())
            {
                var rate = ctx.Rates.First(x => x.Name == name);
                ctx.Rates.Remove(rate);
                ctx.SaveChanges();
            }
        }
    }
}
