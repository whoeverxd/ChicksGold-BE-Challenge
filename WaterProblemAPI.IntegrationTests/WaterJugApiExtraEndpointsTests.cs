using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WaterProblemAPI.IntegrationTests
{
    public class WaterJugApiExtraEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WaterJugApiExtraEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ExampleEndpoint_ReturnsSampleRequestAndResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/WaterJug/example");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("sampleRequest", json);
            Assert.Contains("sampleResponse", json);
        }

        [Fact]
        public async Task LimitsEndpoint_ReturnsLimits()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/WaterJug/limits");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("min", json);
            Assert.Contains("max", json);
        }

        [Fact]
        public async Task ValidateEndpoint_ReturnsSolvableTrue()
        {
            var client = _factory.CreateClient();
            var request = new { X_Capacity = 2, Y_Capacity = 10, Z_Amount_Wanted = 4 };
            var response = await client.PostAsJsonAsync("/api/WaterJug/validate", request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("solvable", json);
            Assert.Contains("true", json);
        }

        [Fact]
        public async Task ValidateEndpoint_ReturnsSolvableFalse()
        {
            var client = _factory.CreateClient();
            var request = new { X_Capacity = 2, Y_Capacity = 6, Z_Amount_Wanted = 5 };
            var response = await client.PostAsJsonAsync("/api/WaterJug/validate", request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("solvable", json);
            Assert.Contains("false", json);
        }
    }
}
