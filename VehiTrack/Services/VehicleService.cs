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
            var createdVehicle = await _vehicleRepository.CreateVehicleAsync(vehicle);
            return createdVehicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var updatedVehicle = await _vehicleRepository.UpdateVehicleAsync(vehicle);
            return updatedVehicle;
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.DeleteVehicleAsync(vehicle);
        }

        public async Task<ICollection<Vehicle>> GetVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetVehiclesAsync();
            return vehicles;
        }

        public async Task<Vehicle?> GetVeicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);

            return vehicle;
        }
    }
}
