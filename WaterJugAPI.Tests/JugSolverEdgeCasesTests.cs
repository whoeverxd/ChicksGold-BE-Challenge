using Xunit;
using WaterJugAPI.Services;

namespace WaterJugAPI.Tests
{
    public class JugSolverEdgeCasesTests
    {
        [Fact]
        public void Solve_ShouldReturnNoSolution_WhenZGreaterThanMax()
        {
            var solver = new JugSolver();
            var result = solver.Solve(2, 3, 10); // Z > max(X, Y)
            Assert.True(result.BestSolution == null || result.BestSolution.Count == 0);
            Assert.Contains("No solution", result.Message);
        }

        [Fact]
        public void Solve_ShouldReturnNoSolution_WhenZNotMultipleOfGCD()
        {
            var solver = new JugSolver();
            var result = solver.Solve(3, 6, 4); // 4 no es múltiplo de gcd(3,6)=3
            Assert.True(result.BestSolution == null || result.BestSolution.Count == 0);
            Assert.Contains("No solution", result.Message);
        }

        [Fact]
        public void Solve_ShouldReturnError_WhenInputBelowMinimum()
        {
            var solver = new JugSolver();
            var result = solver.Solve(0, 5, 2); // X < 1
            Assert.Contains("at least 1", result.Message);
        }

        [Fact]
        public void Solve_ShouldReturnError_WhenInputAboveMaximum()
        {
            var solver = new JugSolver();
            var result = solver.Solve(1001, 5, 2); // X > 1000
            Assert.Contains("too large", result.Message);
        }
    }
}
