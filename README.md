# API Endpoints

| Method | Endpoint               | Description                               |
| ------ | ---------------------- | ----------------------------------------- |
| POST   | /api/WaterJug/solve    | Solve the water jug problem               |
| GET    | /api/WaterJug/example  | Get a sample request and response         |
| GET    | /api/WaterJug/health   | Health check                              |
| GET    | /api/WaterJug/limits   | Get input limits for X, Y, Z              |
| POST   | /api/WaterJug/validate | Check if a problem is solvable (no steps) |

-Check swagger for more info and interactive tests, wink wink ;)

# Example Request/Response

**Request:**
```json
POST /api/WaterJug/solve
{
   "X_Capacity": 2,
   "Y_Capacity": 10,
   "Z_Amount_Wanted": 4
}
```

**Response:**
```json
{
   "allSolutions": [...],
   "bestSolution": [...],
   "worstSolution": [...],
   "message": "Solved"
}
```

# Testing Instructions

**Unit tests:**
```sh
dotnet test WaterJugAPI.Tests
```

**Integration tests:**
```sh
dotnet test WaterProblemAPI.IntegrationTests
```

# Error Handling

| Status | Example Message                                      | When?                         |
| ------ | ---------------------------------------------------- | ----------------------------- |
| 400    | {"message": "All values must be positive integers."} | Invalid input (X, Y, Z < 1)   |
| 200    | {"message": "No solution possible: ..."}             | No solution for given X, Y, Z |

# Design Decisions

- BFS is used for optimal and complete solution finding.
- In-memory caching for performance under repeated queries.
- Input limits to prevent resource exhaustion and abuse.

# Extensibility

- Could add authentication, rate limiting, persistent storage, or user history.
- Could expose more endpoints for statistics or advanced queries.

# Contact

For questions or feedback, contact: whoeverxd (GitHub).

# ChicksGold-BE-Challenge

This API computes the steps required to measure exactly Z gallons using two jugs of capacities X and Y gallons.

## Solution

- Initially, I considered solving the problem using backtracking as mentioned in the interview, but after reading the requirements carefully, I realized it would not be the most efficient approach.
- The application only accepts positive values and will reject negative or zero values. (This could be improved by taking the absolute value of any negative input.)
- The API returns all possible solutions, and explicitly identifies and returns the best (optimal) and worst solutions, not just a single solution as in the sample response.json:

```json
{
    "solution": [
        {"step": 1, "bucketX": 2, "bucketY": 0, "action": "Fill bucket X"},
        {"step": 2, "bucketX": 0, "bucketY": 2, "action": "Transfer from bucket X to Y"},
        {"step": 3, "bucketX": 2, "bucketY": 2, "action": "Fill bucket X"},
        {"step": 4, "bucketX": 0, "bucketY": 4, "action": "Transfer from bucket X to Y", "status": "Solved"}
    ]
}
```

- To provide a more complete and flexible response, the API includes:
  - An array with all possible solutions (each as a list of steps).
  - A field for the best solution (the one with the fewest steps).
  - A field for the worst solution (the one with the most steps).
  - The default response is always the optimal solution, but this can be changed to return the first found solution if needed, covering all three solution schemes (one solution, all solutions, the optimal, and the worst).

## How to Set Up and Run the Application

1. **Clone the repository:**
   ```sh
   git clone https://github.com/whoeverxd/ChicksGold-BE-Challenge.git
   cd ChicksGold-BE-Challenge/WaterProblemAPI
   ```

2. **Restore dependencies:** (MAKE sure you are in the right API folder using cd before continuing) 
   ```sh
   dotnet restore
   ```

3. **Build the project:**
   ```sh
   dotnet build
   ```

4. **Run the API:**
   ```sh
   dotnet run
   ```

5. **Access the API:**
   - By default, the API will be available at `https://localhost:5001` or `http://localhost:5000`.
   - Swagger UI is enabled in development mode at `/swagger` for easy testing.
## RESTful API Requirement 
    - the POST endpoint /api/WaterJug/solve accepts jug capacities (X, Y) and the target volume (Z) as JSON input.
    - Uses POST for problem-solving (since it processes data and is not idempotent).
            Returns 400 Bad Request for invalid input.
            Returns 200 OK with a JSON response for valid requests.
            Returns very clear Responses with solutions and if no solution its possible, it also returns a JSON message explanining WHY
   - 

## Algorithm Explanation

The algorithm uses a breadth-first search (BFS) approach to explore all possible states of the two jugs. Each state is defined by the current amount of water in each jug. The possible actions at each step are:

- Fill either jug to its maximum capacity.
- Empty either jug.
- Transfer water from one jug to the other until one is empty or the other is full.

The BFS ensures that all possible sequences of actions are explored, and all solutions are found. The best solution is the one with the fewest steps, and the worst is the one with the most steps. The API returns all solutions, as well as the best and worst, for maximum flexibility.

## Scalability (Extra Points)

- The API uses in-memory caching (MemoryCache) to store results for common or recent requests, improving performance under high load.


## Error Handling and Validation:
   - The API validates that X, Y, and Z are positive integers. If not, it returns a 400 Bad Request with a clear JSON message: { "message": "All values must be positive integers." }
   - The service also checks for cases where the problem has no solution:
      - If Z > max(X, Y), it is impossible to measure Z (target is larger than both buckets).
      - If Z is not a multiple of gcd(X, Y), it is impossible to measure Z (by Bézout's theorem).
   - In these cases, the service returns a clear message, and the controller responds with: { "message": result.Message } in JSON format.
  
# Performance Considerations:
    - current implementation with BFS and caching is already efficient for most practical input sizes.
      - Input limits are enforced to prevent errors and resource exhaustion:
         - Minimum range: X, Y, Z ≥ 1.
         - Maximum range: X ≤ 1000, Y ≤ 999, Z ≤ 1000.
            - Note: These are not mathematical limits of the problem, but practical ones. Using BFS, the number of possible states is O(x*y). For example, if X = Y = 10,000, you already have 100 million states, which is too large for memory and CPU.
            - This prevents a malicious user like william bello from sending extremely large values (e.g., X=10^9, Y=10^9, Z=1) and crashing the service. Therefore, a limit of 1000 is set for safety.
  
# PROGRAMMER NOTES
   - Worst practical case: when Z is reachable but only after a long path in the state graph.
   - Worst-case complexity: O(X * Y) in both time and memory.
   - A.gitignore file is added to prevent unnecessary files, such as /bin folders from tests, from being tracked in git.
   - the tests are located in the main project folder (root), so make sure of moving there for testing purposes

    ## INTEGRATION TESTS 
        - USING  dotnet add WaterProblemAPI.IntegrationTests package Microsoft.AspNetCore.Mvc.Testing --version 8.0.5

        Command: dotnet test WaterProblemAPI.IntegrationTests
    ## UNIT TESTS 

        Command: dotnet test WaterJugAPI.Tests



    