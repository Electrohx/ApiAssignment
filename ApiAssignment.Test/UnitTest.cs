using ApiAssignment.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiAssignment.Test
{
    public class UnitTest
    {
        [Fact]
        public async Task GetMovieShouldReturnBadRequest()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var res = await client.GetAsync("/movie");

            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }

        [Theory]
        [InlineData("/movie?MovieTitle=titanic")]
        [InlineData("/movie?MovieTitle=titanic&plot=full")]
        public async Task GetMovieShouldReturnOk(string url)
        {
            var waf = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IClient, TestClient>();
                });
            });

            var res = await waf.CreateClient().GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
    }

    public class TestClient : IClient
    {
        public Task<string> GetMovieAsync(Request request)
        {
            return Task.Run(() => string.Empty);
        }
    }
}
