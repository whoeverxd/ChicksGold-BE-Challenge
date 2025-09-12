using Xunit;
using WaterJugAPI.Services;

namespace WaterJugAPI.Tests
{
    public class JugSolverAllSolutionsTests
    {
        [Fact]
        public void Solve_ShouldReturnAllSolutions_AndBestAndWorst()
        {
            var solver = new JugSolver();
            var result = solver.Solve(2, 10, 4);
            Assert.NotNull(result.AllSolutions);
            Assert.True(result.AllSolutions.Count > 0);
            Assert.NotNull(result.BestSolution);
            Assert.NotNull(result.WorstSolution);
            Assert.True(result.BestSolution.Count <= result.WorstSolution.Count);
        }
    }
}
