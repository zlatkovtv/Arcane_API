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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository repo;

        public TodoController(ITodoRepository repository) {
            repo = repository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            if(userId <= 0) {
                return BadRequest();
            }

            var result = await repo.GetAll(userId);
            if(!result.Any()) {
                return NotFound(userId);
            }

            return Ok(result);
        }

        [ValidateModelAttribute]
        [HttpPost]
        public ActionResult Post([FromBody] Todo todo)
        {
            this.repo.Add(todo);
            return StatusCode(Status201Created);
        }

        [ValidateModelAttribute]
        [HttpPut]
        public ActionResult Put([FromBody] Todo todo)
        {
            var rowsAffected = this.repo.Update(todo);
            if(rowsAffected == 0) {
                return NotFound(todo.Id);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if(id <= 0) {
                return BadRequest();
            }

            var rowsAffected = this.repo.Delete(id);
            if(rowsAffected == 0) {
                return NotFound(id);
            }

            return Ok();
        }
    }
}
