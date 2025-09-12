namespace WaterJugAPI.Models
{
    public class JugResponse
    {
        public List<JugStep> Solution { get; set; } = new();
        public string Message { get; set; } = "";
    }
}