using VehiTrack.Models;
using VehiTrack.Repositories;

namespace VehiTrack.Services
{
    public class VheicleServices
    {
        private readonly VehicleRepository _vheicleRepository;

        public VheicleServices()
        {
            _vheicleRepository = new VehicleRepository();
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            var createVheicle = await _vheicleRepository.CreateVehicleAsync(vehicle);
            return createVheicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var updateVehicle = await _vheicleRepository.UpdateVehicleAsync(vehicle);
            return updateVehicle;
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            await _vheicleRepository.DeleteVehicleAsync(vehicle);
        }

        public async Task<ICollection<Vehicle>> GetVehiclesAsync()
        {
            var vheicles = await _vheicleRepository.GetVehiclesAsync();
            return vheicles;
        }
    }
}
