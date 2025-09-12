namespace WaterJugAPI.Models
{
    public class JugStep
    {
        public int Step { get; set; }
        public int BucketX { get; set; }
        public int BucketY { get; set; }
        public string Action { get; set; } = "";
        public string? Status { get; set; }
    }
}