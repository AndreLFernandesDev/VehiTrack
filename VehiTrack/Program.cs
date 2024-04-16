using VehiTrack.Models;
using VehiTrack.Services;

namespace VehiTrack
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var userService = new UserService();

            /************************************************************************
             * Insert User
             ************************************************************************/

            // var user = new User() { Name = "Maycon Sousa", Email = "mayconfsousa@gmail.com" };
            // user = await userService.CreateUserAsync(user);

            // Console.WriteLine($"Created user: [{user.Id}] {user.Email}");

            /************************************************************************
             * Get User by Email
             ************************************************************************/

            // var user = await userService.GetUserByEmailAsync("mayconfsousa@gmail.com");

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

            // var user = await userService.GetUserByEmailAsync("mayconfsousa@gmail.com");

            // if (user != null)
            // {
            //     user.Name = "Maycon Sousa 2";

            //     user = await userService.UpdateUserAsync(user);

            //     Console.WriteLine($"User updated: [{user.Id}] {user.Email}");
            // }
            // else
            // {
            //     Console.WriteLine("User not found");
            // }

            /************************************************************************
             * Get All Users
             ************************************************************************/

            var users = await userService.GetUsersAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"User: [{user.Id}] {user.Email}");
            }
        }
    }
}
