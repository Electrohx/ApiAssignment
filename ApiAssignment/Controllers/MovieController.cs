using ApiAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiAssignment
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IClient client;

        public MovieController(IClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Request req)
        {
            if (string.IsNullOrEmpty(req.MovieTitle))
            {
                return BadRequest("omdbapi will return error if not title is included");
            }

            var apiResponse = await client.GetMovieAsync(req);

            return Ok(apiResponse);
        }
    }
}
