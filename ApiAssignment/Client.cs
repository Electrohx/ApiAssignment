using ApiAssignment.Models;
using ApiAssignment.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiAssignment
{
    public interface IClient
    {
        Task<string> GetMovieAsync(Request request);
    }

    public class Client : IClient
    {
        private readonly IOptions<ApiAssignmentOptions> options;

        public Client(IOptions<ApiAssignmentOptions> options)
        {
            this.options = options;
        }

        public async Task<string> GetMovieAsync(Request request)
        {
            var queryParams = GetParams(request);

            var url = QueryHelpers.AddQueryString("http://www.omdbapi.com/", queryParams);
            string apiResponse;

            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(url))
            {
                apiResponse = await response.Content.ReadAsStringAsync();
            }

            return apiResponse;
        }

        private Dictionary<string, string> GetParams(Request re)
        {
            var queryParams = new Dictionary<string, string>();

            queryParams.Add("apikey", options.Value.ApiKey);

            //queryParams.Add("r", "json");

            if (!string.IsNullOrEmpty(re.MovieTitle))
            {
                queryParams.Add("t", re.MovieTitle);
            }

            if (re.MovieYear.HasValue)
            {
                queryParams.Add("y", re.MovieYear.Value.ToString());
            }

            if (re.MoviePlot.HasValue)
            {
                queryParams.Add("plot", re.MoviePlot.Value.ToString());
            }

            return queryParams;
        }
    }
}
