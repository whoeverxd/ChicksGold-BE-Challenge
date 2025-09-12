namespace WaterJugAPI.Models
{
    public class JugResponse
    {
        // Todas las soluciones posibles
        public List<List<JugStep>> AllSolutions { get; set; } = new();

        // Mejor solución (menos pasos)
        public List<JugStep> BestSolution { get; set; } = new();

        // Peor solución (más pasos)
        public List<JugStep> WorstSolution { get; set; } = new();

        // Mensaje informativo
        public string Message { get; set; } = "";
    }
}