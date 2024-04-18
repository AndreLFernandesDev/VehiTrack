using Microsoft.EntityFrameworkCore;
using VehiTrack.Models;

namespace VehiTrack.Repositories
{
    public class VehicleRepository
    {
        private readonly AppContext _ctx;

        public VehicleRepository()
        {
            _ctx = new AppContext();
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            _ctx.Vehicles.Add(vehicle);
            await _ctx.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            _ctx.Vehicles.Update(vehicle);
            await _ctx.SaveChangesAsync();
            return vehicle;
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            _ctx.Vehicles.Remove(vehicle);
            await _ctx.SaveChangesAsync();
        }

        public async Task<ICollection<Vehicle>> GetVehiclesAsync()
        {
            var vehicles = await _ctx.Vehicles.ToListAsync();
            return vehicles;
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _ctx.Vehicles.FindAsync(id);
            return vehicle;
        }
    }
}
