using Microsoft.AspNetCore.Mvc;
using WaterJugAPI.Models;
using WaterJugAPI.Services;

namespace WaterJugAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterJugController : ControllerBase
    {
        private readonly JugSolver _solver = new();

        [HttpPost("solve")]
        public ActionResult<JugResponse> SolveJugProblem([FromBody] JugRequest request)
        {
            if (request.X_Capacity <= 0 || request.Y_Capacity <= 0 || request.Z_Amount_Wanted <= 0)
                return BadRequest(new { message = "All values must be positive integers." });

            var result = _solver.Solve(request.X_Capacity, request.Y_Capacity, request.Z_Amount_Wanted);

            if (!result.Solution.Any())
                return Ok(new { message = result.Message });

            return Ok(result);
        }
    }
}
