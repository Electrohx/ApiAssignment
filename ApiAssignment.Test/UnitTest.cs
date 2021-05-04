using ApiAssignment.Models;
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
            var client = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IClient, JsonClient>();
                    });
                }).CreateClient();

            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory]
        [InlineData("/movie?Title=NotTitanic")]
        [InlineData("/movie?Title=NotTitanic&Year=1911")]
        public async Task GetMovieWithTestClientShouldReturnNotFound(string url)
        {
            var client = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IClient, NotFoundClient>();
                    });
                }).CreateClient();

            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }

        [Fact]
        public async Task GetMovieShouldReturnBadRequestWhenClientThrowsException()
        {
            var client = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IClient, ArgumentExceptionClient>();
                    });
                }).CreateClient();

            var res = await client.GetAsync("/movie?Title=title");

            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }

        private static System.Net.Http.HttpClient GetClient()
        {
            return new WebApplicationFactory<Startup>().CreateClient();
        }
    }

    public class ArgumentExceptionClient : IClient
    {
        public Task<string> GetMovieAsync(MovieRequestDTO request)
        {
            throw new ArgumentException();
        }
    }

    public class JsonClient : IClient
    {
        public Task<string> GetMovieAsync(MovieRequestDTO request)
        {
            return Task.Run(() => "{\"Title\":\"Titanic\"}");
        }
    }

    public class NotFoundClient : IClient
    {
        public Task<string> GetMovieAsync(MovieRequestDTO request)
        {
            return Task.Run(() => "Movie not found!");
        }
    }
}
