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
        public async Task<IActionResult> Get([FromQuery] MovieRequestDTO req)
        {
            if (string.IsNullOrEmpty(req.Title))
            {
                return BadRequest("omdbapi will return error if title is not included");
            }

            return await GetMovie(req);
        }

        private async Task<IActionResult> GetMovie(MovieRequestDTO req)
        {
            try
            {
                var apiResponse = await client.GetMovieAsync(req);
                return Ok(apiResponse);
            }
            catch (System.ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
