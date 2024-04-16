using VehiTrack.Models;
using VehiTrack.Repositories;

namespace VehiTrack.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var createdUser = await _userRepository.CreateUserAsync(user);

            return createdUser;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var updatedUser = await _userRepository.UpdateUserAsync(user);

            return updatedUser;
        }

        public async Task DeleteUserAsync(User user)
        {
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();

            return users;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            return user;
        }
    }
}
