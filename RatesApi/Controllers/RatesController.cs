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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok();
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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RatesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
