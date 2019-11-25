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
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoRepository repo;

        public CryptoController(ICryptoRepository repository) {
            repo = repository;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await repo.GetCryptoInfo();
            if(result == null) {
                return NotFound($"No crypto data.");
            }

            return Ok(result);
        }
    }
}
