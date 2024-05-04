using Microsoft.EntityFrameworkCore;
using VehiTrack.Models;

namespace VehiTrack
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RefuelingRecord> RefuelingRecords { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(AppSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /************************************************************************
             * TABLE: users
             ************************************************************************/

            builder.Entity<User>().ToTable("users");

            // COLUMN: id
            builder.Entity<User>().Property(u => u.Id).HasColumnName("id").UseIdentityColumn();

            // COLUMN: first_name
            builder
                .Entity<User>()
                .Property(u => u.FirstName)
                .HasColumnName("first_name")
                .IsRequired();

            // COLUMN: last_name
            builder
                .Entity<User>()
                .Property(u => u.LastName)
                .HasColumnName("last_name")
                .IsRequired();

            // COLUMN: username
            builder
                .Entity<User>()
                .Property(u => u.Username)
                .HasColumnName("username")
                .IsRequired();
            builder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            // COLUMN: telegram_id
            builder
                .Entity<User>()
                .Property(u => u.TelegramId)
                .HasColumnName("telegram_id")
                .IsRequired();
            builder.Entity<User>().HasIndex(u => u.TelegramId).IsUnique();

            // RELATIONSHIP: vehicles -> users
            builder
                .Entity<User>()
                .HasMany(u => u.Vehicles)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId);

            /**************************************************************
             * TABLE: vehicles
             **************************************************************/

            builder.Entity<Vehicle>().ToTable("vehicles");

            // COLUMN: id
            builder.Entity<Vehicle>().Property(v => v.Id).HasColumnName("id").UseIdentityColumn();

            // COLUMN: name
            builder.Entity<Vehicle>().Property(v => v.Name).HasColumnName("name").IsRequired();

            builder.Entity<Vehicle>().HasIndex(v => new { v.UserId, v.Name }).IsUnique();

            // COLUMN: user_id
            builder
                .Entity<Vehicle>()
                .Property(v => v.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            // RELATIONSHIP: refueling_records -> vehicles
            builder
                .Entity<Vehicle>()
                .HasMany(v => v.RefuelingRecords)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId);

            /***************************************************************
             * TABLE: refueling_records
             ***************************************************************/

            builder.Entity<RefuelingRecord>().ToTable("refueling_records");

            // COLUMN: id
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            // COLUMN: date_time
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.DateTime)
                .HasColumnName("date_time")
                .IsRequired();

            // COLUMN: odometer_counter
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.OdometerCounter)
                .HasColumnName("odometer_counter")
                .IsRequired();

            // COLUMN: is_full
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.IsFull)
                .HasColumnName("is_full")
                .IsRequired();

            // CULUMN: quantity
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            // COLUMN: unity_price
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.UnitPrice)
                .HasColumnName("unity_price")
                .IsRequired();

            // COLUMN: total_cost
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.TotalCost)
                .HasColumnName("total_cost")
                .HasComputedColumnSql("quantity * unity_price", true)
                .IsRequired();

            // COLUMN: fuel_type_id
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.FuelTypeId)
                .HasColumnName("fuel_type_id")
                .IsRequired();

            // COLUMN: vehicle_id
            builder
                .Entity<RefuelingRecord>()
                .Property(r => r.VehicleId)
                .HasColumnName("vehicle_id")
                .IsRequired();

            /**************************************************************************************
             * TABLE: fuel_types
             **************************************************************************************/

            builder.Entity<FuelType>().ToTable("fuel_types");

            // COLUMN: id
            builder.Entity<FuelType>().Property(f => f.Id).HasColumnName("id").UseIdentityColumn();

            // COLUMN: name
            builder.Entity<FuelType>().Property(f => f.Name).HasColumnName("name").IsRequired();

            // RELATIONSHIP: refueling_records -> fuel_types
            builder
                .Entity<FuelType>()
                .HasMany(f => f.RefuelingRecords)
                .WithOne(r => r.FuelType)
                .HasForeignKey(r => r.FuelTypeId);

            base.OnModelCreating(builder);
        }
    }
}
