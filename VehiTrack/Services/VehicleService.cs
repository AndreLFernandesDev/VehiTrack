using VehiTrack.Models;
using VehiTrack.Repositories;

namespace VehiTrack.Services
{
    public class VehicleService
    {
        private readonly VehicleRepository _vehicleRepository;

        public VehicleService()
        {
            _vehicleRepository = new VehicleRepository();
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            var createVheicle = await _vehicleRepository.CreateVehicleAsync(vehicle);
            return createVheicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var updateVehicle = await _vehicleRepository.UpdateVehicleAsync(vehicle);
            return updateVehicle;
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.DeleteVehicleAsync(vehicle);
        }

        public async Task<ICollection<Vehicle>> GetVehiclesAsync()
        {
            var vheicles = await _vehicleRepository.GetVehiclesAsync();
            return vheicles;
        }

        public async Task<Vehicle?> GetVeicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);

            return vehicle;
        }
    }
}
