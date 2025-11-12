using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolResultSystem.Web;

namespace SchoolResultSystem.Tests
{
    public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EndpointTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task HomePage_ShouldReturn_Success()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("School", content);
        }

        [Fact]
        public async Task CopyDbEndpoint_ReturnsRedirectOrFile()
        {
            var response = await _client.GetAsync("/Principal/SetupPrincipal/CopyDb");
            Assert.True(response.IsSuccessStatusCode || (int)response.StatusCode == 302);
        }
    }
}
