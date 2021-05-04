using ApiAssignment.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
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
            System.Net.Http.HttpClient client = GetClient();

            var res = await client.GetAsync("/movie");

            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }

        [Fact]
        public async Task GetMovieShouldReturnNotFound()
        {
            System.Net.Http.HttpClient client = GetClient();

            var res = await client.GetAsync("/movie?Title=ThisIsNotAValidTitle");

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }

        [Theory]
        [InlineData("/movie?Title=Titanic")]
        [InlineData("/movie?Title=Titanic&Plot=full")]
        public async Task GetMovieWithTestClientShouldReturnOk(string url)
        {
            var client = GetClient(true);

            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory]
        [InlineData("/movie?Title=NotTitanic")]
        [InlineData("/movie?Title=NotTitanic&Year=1911")]
        public async Task GetMovieWithTestClientShouldReturnNotFound(string url)
        {
            var client = GetClient(true);

            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }

        private static System.Net.Http.HttpClient GetClient(bool mockedClient = false)
        {
            var waf = new WebApplicationFactory<Startup>();

            return mockedClient ? 
                waf.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IClient, TestClient>();
                    });
                }).CreateClient()
                :
                waf.CreateClient();
        }
    }

    public class TestClient : IClient
    {
        public Task<string> GetMovieAsync(MovieRequestDTO request)
        {
            return request.Title.ToLower() == "titanic" ? Task.Run(() => "{\"Title\":\"Titanic\"}") : Task.Run(() => "Movie not found!");
        }
    }
}
