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
        public int FuelTypeId { get; set; }
        public virtual required FuelType FuelType { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual required Vehicle Vehicle { get; set; }
    }
}
