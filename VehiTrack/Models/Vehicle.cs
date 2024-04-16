namespace VehiTrack.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }
        public virtual required User User { get; set; }
        public virtual required ICollection<RefuelingRecord> RefuelingRecords { get; set; }
    }
}
