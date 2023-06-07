using DatabaseRepository.Models;
using DatabaseRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using RatesApi.Managers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RatesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly RatesRepository ratesRepository;
        public RatesController()
        {
            ratesRepository = new RatesRepository();
        }
        // GET: api/<RatesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(ratesRepository.GetRates());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // GET api/<RatesController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                return Ok(ratesRepository.GetRates().First(x => x.Name == name));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // POST api/<RatesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RateModel rate)
        {
            try
            {
                var currentRate = (await RatesManager.GetRates()).FirstOrDefault(x => x.Name == rate.Name);

                if (currentRate != null)
                {
                    ratesRepository.AddRate(new RateModel { Name = rate.Name, ExchangeRate = currentRate.ExchangeRate, Rate = currentRate.Rate });
                    return Ok("Rate was added");
                }
                throw new Exception("Rates doesn't exist");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT api/<RatesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string name)
        {
            try
            {
                var newRate = (await RatesManager.GetRates()).FirstOrDefault(x => x.Name == name);

                if (newRate != null)
                {
                    ratesRepository.UpdateRate(new RateModel { Name = newRate.Name, ExchangeRate = newRate.ExchangeRate, Rate = newRate.Rate }, ratesRepository.GetRates().First(x => x.Id == id));
                    return Ok("Rate was updated");
                }
                throw new Exception("Rates doesn't exist");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<RatesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                ratesRepository.DeleteRate(name);
                return Ok("Rate was deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
