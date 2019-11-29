using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ArcaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherRepository repo;

        public WeatherController(IWeatherRepository repository) {
            repo = repository;
        }

        [HttpGet("{city?}/{country?}")]
        public async Task<ActionResult> Get(string city = "London", string country = "UK")
        {
            if(string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country)) {
                return BadRequest();
            }

            var result = await repo.GetForecastForCity(city, country);
            if(result == null) {
                return NotFound($"Weather for {city}, {country} not found.");
            }

            return Ok(result);
        }
    }
}
