namespace VehiTrack.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public required long TelegramId { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; } = [];
    }
}
