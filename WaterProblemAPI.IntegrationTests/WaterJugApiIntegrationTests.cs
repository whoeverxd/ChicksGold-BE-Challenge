using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WaterProblemAPI.IntegrationTests
{
    public class WaterJugApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WaterJugApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SolveEndpoint_ReturnsSuccessAndSolution()
        {
            var client = _factory.CreateClient();
            var request = new { X_Capacity = 2, Y_Capacity = 10, Z_Amount_Wanted = 4 };
            var response = await client.PostAsJsonAsync("/api/WaterJug/solve", request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("bestSolution", json.ToLower());
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsHealthy()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/WaterJug/health");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", json);
        }
    }
}
