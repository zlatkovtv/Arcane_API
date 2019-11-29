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

        [HttpGet("{source?}")]
        public async Task<ActionResult> Get(string source)
        {
            var result = await repo.GetNewsInfo(source);
            if(result == null) {
                return NotFound($"No news data.");
            }

            return Ok(result);
        }

        [HttpGet("sources")]
        public async Task<ActionResult> GetSources()
        {
            var result = await repo.GetNewsSources();
            if(result == null) {
                return NotFound($"No news sources.");
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
