using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VehiTrack.Models;

namespace VehiTrack
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .Build();

            var connectionString = config["ConnectionString"];

            builder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /************************************************************************
             * TABLE: users
             ************************************************************************/

            builder.Entity<User>().ToTable("users");

            // COLUMN: id
            builder.Entity<User>().Property(u => u.Id).HasColumnName("id").UseIdentityColumn();

            // COLUMN: name
            builder.Entity<User>().Property(u => u.Name).HasColumnName("name").IsRequired();

            // COLUMN: email
            builder.Entity<User>().Property(u => u.Email).HasColumnName("email").IsRequired();
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
