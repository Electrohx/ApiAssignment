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
        private readonly HttpClient client;

        public Client(IOptions<ApiAssignmentOptions> options, IHttpClientFactory clientFactory)
        {
            this.options = options;
            this.client = clientFactory.CreateClient();
        }

        public async Task<string> GetMovieAsync(MovieRequestDTO request)
        {
            var queryParams = GetParameters(request);

            if (!queryParams.ContainsKey("apikey"))
            {
                throw new ArgumentException("apikey was not defined");
            }

            var url = QueryHelpers.AddQueryString("http://www.omdbapi.com/", queryParams);

            var respons = await client.GetAsync(url);
            
            if (respons.IsSuccessStatusCode)
            {
                return respons.Content.ReadAsStringAsync()?.Result;
            }
            else
            {
                return string.Empty;
            }
        }

        private Dictionary<string, string> GetParameters(MovieRequestDTO re)
        {
            var queryParams = new Dictionary<string, string>();

            queryParams.Add("apikey", options.Value.ApiKey);

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
