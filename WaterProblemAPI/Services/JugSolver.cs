using WaterJugAPI.Models;

namespace WaterJugAPI.Services
{
    public class JugSolver
    {
        public JugResponse Solve(int x, int y, int z)
        {
            var response = new JugResponse();

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

            // Ejecutar dos simulaciones: empezando con X y empezando con Y
            var fromX = Pour(x, y, z, "X", "Y");
            var fromY = Pour(y, x, z, "Y", "X");

            // Escoger la mejor (menos pasos)
            var best = fromX.Count <= fromY.Count ? fromX : fromY;

            response.Solution = best;
            response.Message = "Solved";
            return response;
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
