using ApiAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            return await GetMovie(req);
        }

        private async Task<IActionResult> GetMovie(MovieRequestDTO req)
        {
            try
            {
                var apiResponse = await client.GetMovieAsync(req);

                if (apiResponse.Contains("Movie not found!"))
                {
                    return NotFound();
                }

                return Ok(JsonConvert.DeserializeObject<MovieResponseDTO>(apiResponse));
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
