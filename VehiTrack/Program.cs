namespace VehiTrack
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new TelegramBot();

            await bot.StartAsync();

            // await using var ctx = new AppContext();

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
}
