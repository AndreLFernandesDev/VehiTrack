namespace VehiTrack.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<RefuelingRecord> RefuelingRecords { get; set; } = [];
    }
}
