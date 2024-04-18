namespace VehiTrack.Models
{
    public class RefuelingRecord
    {
        public int Id { get; set; }
        public required DateOnly Date { get; set; }
        public required int OdometerCounter { get; set; }
        public required bool IsFull { get; set; }
        public required double Quantity { get; set; }
        public required double UnitPrice { get; set; }
        public double TotalCost { get; set; }
        public required int FuelTypeId { get; set; }
        public virtual FuelType FuelType { get; set; } = null!;
        public required int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
