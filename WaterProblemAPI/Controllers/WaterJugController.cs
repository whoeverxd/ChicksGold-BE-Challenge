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
    }
}
