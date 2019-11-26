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
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository repo;

        public NewsController(INewsRepository repository) {
            repo = repository;
        }

        [HttpGet("{country?}")]
        public async Task<ActionResult> Get(string country = "us")
        {
            var result = await repo.GetNewsInfo(country);
            if(result == null) {
                return NotFound($"No news data.");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddToFavourites(NewsAction newsAction) {
            var result = await repo.AddToFavourites(newsAction);
            if(result == null) {
                return StatusCode(500);
            }

            return Ok(result);
        }
    }
}
