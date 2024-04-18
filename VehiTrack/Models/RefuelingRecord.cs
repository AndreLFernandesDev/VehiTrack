namespace VehiTrack.Models
{
    public class RefuelingRecord
    {
        public int Id { get; set; }
        public required DateOnly Date { get; set; }
        public required int Odometer { get; set; }
        public required bool IsFull { get; set; }
        public required double Amount { get; set; }
        public required double Price { get; set; }
        public double TotalPrice { get; set; }
        public required int FuelTypeId { get; set; }
        public virtual FuelType FuelType { get; set; } = null!;
        public required int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
