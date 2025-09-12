using WaterJugAPI.Models;

namespace WaterJugAPI.Services
{
    public class JugSolver
    {
        public JugResponse Solve(int x, int y, int z)
        {
            var response = new JugResponse();

            // Input limits: min 1, max 1000 for x and z, max 999 for y
            if (x < 1 || y < 1 || z < 1)
            {
                response.Message = "Input values must be at least 1 (X, Y, Z ≥ 1).";
                return response;
            }
            if (x > 1000 || y > 999 || z > 1000)
            {
                response.Message = "Input values too large. Max allowed: X ≤ 1000, Y ≤ 999, Z ≤ 1000. This prevents excessive memory/CPU usage.";
                return response;
            }

            if (z > Math.Max(x, y))
            {
                response.Message = "No solution possible: target is larger than both buckets.";
                return response;
            }

            if (z % Gcd(x, y) != 0)
            {
                response.Message = "No solution possible: target is not divisible by GCD of capacities.";
                return response;
            }

            // Buscar todas las soluciones posibles usando BFS
            var allSolutions = FindAllSolutions(x, y, z);
            response.AllSolutions = allSolutions;

            if (allSolutions.Count == 0)
            {
                response.Message = "No solution found.";
                return response;
            }

            // Mejor solución: menos pasos
            var best = allSolutions.OrderBy(s => s.Count).First();
            // Peor solución: más pasos
            var worst = allSolutions.OrderByDescending(s => s.Count).First();

            response.BestSolution = best;
            response.WorstSolution = worst;
            response.Message = "Solved";
            return response;
        }

        // BFS para encontrar todas las soluciones posibles
        private List<List<JugStep>> FindAllSolutions(int x, int y, int z)
        {
            var solutions = new List<List<JugStep>>();
            var visited = new HashSet<string>();
            var queue = new Queue<(int, int, List<JugStep>)>();

            // Estado inicial: ambos vacíos
            queue.Enqueue((0, 0, new List<JugStep>()));

            while (queue.Count > 0)
            {
                var (currX, currY, steps) = queue.Dequeue();
                string stateKey = $"{currX},{currY}";
                if (visited.Contains(stateKey)) continue;
                visited.Add(stateKey);

                // Si se alcanza la meta
                if (currX == z || currY == z)
                {
                    var solvedSteps = new List<JugStep>(steps);
                    if (solvedSteps.Count > 0)
                        solvedSteps[solvedSteps.Count - 1].Status = "Solved";
                    solutions.Add(solvedSteps);
                    continue;
                }

                int stepNum = steps.Count + 1;

                // Posibles movimientos
                var nextStates = new List<(int, int, string)> {
                    (x, currY, "Fill bucket X"),
                    (currX, y, "Fill bucket Y"),
                    (0, currY, "Empty bucket X"),
                    (currX, 0, "Empty bucket Y"),
                    // Transfer X -> Y
                    (currX - Math.Min(currX, y - currY), currY + Math.Min(currX, y - currY), "Transfer from X to Y"),
                    // Transfer Y -> X
                    (currX + Math.Min(currY, x - currX), currY - Math.Min(currY, x - currX), "Transfer from Y to X")
                };

                foreach (var (nextX, nextY, action) in nextStates)
                {
                    // Evitar ciclos
                    string nextKey = $"{nextX},{nextY}";
                    if (visited.Contains(nextKey)) continue;

                    var newSteps = new List<JugStep>(steps)
                    {
                        new JugStep
                        {
                            Step = stepNum,
                            BucketX = nextX,
                            BucketY = nextY,
                            Action = action
                        }
                    };
                    queue.Enqueue((nextX, nextY, newSteps));
                }
            }
            return solutions;
        }

        private List<JugStep> Pour(int fromCap, int toCap, int target, string fromName, string toName)
        {
            int from = fromCap, to = 0, stepCount = 1;
            var steps = new List<JugStep>
            {
                new JugStep { Step = stepCount++, BucketX = fromName == "X" ? from : to, BucketY = fromName == "Y" ? from : to, Action = $"Fill bucket {fromName}" }
            };

            while (from != target && to != target)
            {
                int transfer = Math.Min(from, toCap - to);
                to += transfer;
                from -= transfer;

                steps.Add(new JugStep
                {
                    Step = stepCount++,
                    BucketX = fromName == "X" ? from : to,
                    BucketY = fromName == "Y" ? from : to,
                    Action = $"Transfer from {fromName} to {toName}"
                });

                if (from == target || to == target)
                {
                    steps.Last().Status = "Solved";
                    break;
                }

                if (from == 0)
                {
                    from = fromCap;
                    steps.Add(new JugStep
                    {
                        Step = stepCount++,
                        BucketX = fromName == "X" ? from : to,
                        BucketY = fromName == "Y" ? from : to,
                        Action = $"Fill bucket {fromName}"
                    });
                }

                if (to == toCap)
                {
                    to = 0;
                    steps.Add(new JugStep
                    {
                        Step = stepCount++,
                        BucketX = fromName == "X" ? from : to,
                        BucketY = fromName == "Y" ? from : to,
                        Action = $"Empty bucket {toName}"
                    });
                }
            }

            return steps;
        }

        private int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);
    }
}
