using Microsoft.AspNetCore.Mvc;
using WaterJugAPI.Models;
using WaterJugAPI.Services;
using Microsoft.Extensions.Caching.Memory;

namespace WaterJugAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterJugController : ControllerBase
    {
        private readonly JugSolver _solver = new();
        private readonly IMemoryCache _cache;

        public WaterJugController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost("solve")]
        public ActionResult<JugResponse> SolveJugProblem([FromBody] JugRequest request)
        {
            if (request.X_Capacity <= 0 || request.Y_Capacity <= 0 || request.Z_Amount_Wanted <= 0)
                return BadRequest(new { message = "All values must be positive integers." });

            string cacheKey = $"{request.X_Capacity}-{request.Y_Capacity}-{request.Z_Amount_Wanted}";
            if (_cache.TryGetValue(cacheKey, out JugResponse? cachedResult) && cachedResult != null)
            {
                return Ok(cachedResult);
            }

            var result = _solver.Solve(request.X_Capacity, request.Y_Capacity, request.Z_Amount_Wanted);

            if (result.BestSolution == null || !result.BestSolution.Any())
                return Ok(new { message = result.Message });

            // Guardar en cache por 10 minutos
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            return Ok(result);
        }
        // GET /api/WaterJug/example
        [HttpGet("example")]
        public IActionResult GetExample()
        {
            var sampleRequest = new JugRequest
            {
                X_Capacity = 2,
                Y_Capacity = 10,
                Z_Amount_Wanted = 4
            };
            var sampleResponse = new JugResponse
            {
                BestSolution = new List<JugStep>
                {
                    new JugStep { Step = 1, BucketX = 2, BucketY = 0, Action = "Fill bucket X" },
                    new JugStep { Step = 2, BucketX = 0, BucketY = 2, Action = "Transfer from bucket X to bucket Y" },
                    new JugStep { Step = 3, BucketX = 2, BucketY = 2, Action = "Fill bucket X" },
                    new JugStep { Step = 4, BucketX = 0, BucketY = 4, Action = "Transfer from bucket X to bucket Y", Status = "Solved" }
                },
                Message = "Solved"
            };
            return Ok(new { sampleRequest, sampleResponse });
        }

        // GET /api/WaterJug/health
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy" });
        }

        // GET /api/WaterJug/limits
        [HttpGet("limits")]
        public IActionResult GetLimits()
        {
            return Ok(new
            {
                min = new { X = 1, Y = 1, Z = 1 },
                max = new { X = 1000, Y = 999, Z = 1000 }
            });
        }

        // POST /api/WaterJug/validate
        [HttpPost("validate")]
        public IActionResult ValidateJugProblem([FromBody] JugRequest request)
        {
            var result = _solver.Solve(request.X_Capacity, request.Y_Capacity, request.Z_Amount_Wanted);
            bool solvable = result.BestSolution != null && result.BestSolution.Any();
            return Ok(new
            {
                solvable,
                message = result.Message
            });
        }
    }
}
