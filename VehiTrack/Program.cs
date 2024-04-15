using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace VehiTrack
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await using var ctx = new AppContext();

            /************************************************************************
             * Insert User
             ************************************************************************/

            // var user = new User() { Name = "Maycon Sousa", Email = "mayconfsousa@gmail.com" };
            // var user = new User() { Name = "André Fernandes", Email = "dedeluiz3040@gmail.com" };

            // ctx.Users.Add(user);

            // await ctx.SaveChangesAsync();

            // Console.WriteLine($"Created user: [{user.Id}] {user.Email}");

            /************************************************************************
             * Get User by Email
             ************************************************************************/

            // var user = await ctx
            //     .Users.Where(u => u.Email == "mayconfsousa@gmail.com")
            //     .FirstOrDefaultAsync();

            // if (user != null)
            // {
            //     Console.WriteLine($"User: [{user.Id}] {user.Email}");
            // }
            // else
            // {
            //     Console.WriteLine("User not found");
            // }

            /************************************************************************
             * Update User
             ************************************************************************/

            // var user = await ctx
            //     .Users.Where(u => u.Email == "mayconfsousa@gmail.com")
            //     .FirstOrDefaultAsync();

            // if (user != null)
            // {
            //     user.Name = "Maycon Sousa 2";

            //     await ctx.SaveChangesAsync();

            //     Console.WriteLine($"User updated: [{user.Id}] {user.Email}");
            // }
            // else
            // {
            //     Console.WriteLine("User not found");
            // }

            /************************************************************************
             * Get All Users
             ************************************************************************/

            // var users = await ctx.Users.ToListAsync();

            // foreach (var user in users)
            // {
            //     Console.WriteLine($"User: [{user.Id}] {user.Email}");
            // }
        }
    }

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

    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
