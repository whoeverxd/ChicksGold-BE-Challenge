
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

2. **Restore dependencies:**
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

## Algorithm Explanation

The algorithm uses a breadth-first search (BFS) approach to explore all possible states of the two jugs. Each state is defined by the current amount of water in each jug. The possible actions at each step are:

- Fill either jug to its maximum capacity.
- Empty either jug.
- Transfer water from one jug to the other until one is empty or the other is full.

The BFS ensures that all possible sequences of actions are explored, and all solutions are found. The best solution is the one with the fewest steps, and the worst is the one with the most steps. The API returns all solutions, as well as the best and worst, for maximum flexibility.

## Scalability (Extra Points)

- The API uses in-memory caching (MemoryCache) to store results for common or recent requests, improving performance under high load.
