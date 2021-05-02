using ApiAssignment.Models;
using ApiAssignment.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiAssignment
{
    public interface IClient
    {
        Task<string> GetMovieAsync(MovieRequestDTO request);
    }

    public class Client : IClient
    {
        private readonly IOptions<ApiAssignmentOptions> options;

        public Client(IOptions<ApiAssignmentOptions> options)
        {
            this.options = options;
        }

        public async Task<string> GetMovieAsync(MovieRequestDTO request)
        {
            var queryParams = GetParams(request);

            if (!queryParams.ContainsKey("apikey"))
            {
                throw new ArgumentException("apikey was not defined");
            }

            var url = QueryHelpers.AddQueryString("http://www.omdbapi.com/", queryParams);
            string apiResponse;

            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(url))
            {
                apiResponse = await response.Content.ReadAsStringAsync();
            }

            return apiResponse;
        }

        private Dictionary<string, string> GetParams(MovieRequestDTO re)
        {
            var queryParams = new Dictionary<string, string>();

            queryParams.Add("apikey", options.Value.ApiKey);

            //queryParams.Add("r", "json");

            if (!string.IsNullOrEmpty(re.Title))
            {
                queryParams.Add("t", re.Title);
            }

            if (re.Year.HasValue)
            {
                queryParams.Add("y", re.Year.Value.ToString());
            }

            if (re.Plot.HasValue)
            {
                queryParams.Add("plot", re.Plot.Value.ToString());
            }

            return queryParams;
        }
    }
}
