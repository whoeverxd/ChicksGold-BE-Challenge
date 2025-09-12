using Xunit;
using WaterJugAPI.Services;

namespace WaterJugAPI.Tests
{
    public class JugSolverInputValidationTests
    {
        [Theory]
        [InlineData(0, 5, 2)]
        [InlineData(5, 0, 2)]
        [InlineData(5, 5, 0)]
        public void Solve_ShouldReturnError_WhenAnyInputIsZero(int x, int y, int z)
        {
            var solver = new JugSolver();
            var result = solver.Solve(x, y, z);
            Assert.Contains("at least 1", result.Message);
        }

        [Theory]
        [InlineData(1001, 5, 2)]
        [InlineData(5, 1000, 2)]
        [InlineData(5, 5, 1001)]
        public void Solve_ShouldReturnError_WhenAnyInputIsTooLarge(int x, int y, int z)
        {
            var solver = new JugSolver();
            var result = solver.Solve(x, y, z);
            Assert.Contains("too large", result.Message);
        }
    }
}
