namespace VehiTrack.Models
{
    public class FuelType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<RefuelingRecord> RefuelingRecords { get; set; } = [];
    }
}
