using Microsoft.EntityFrameworkCore;
using VehiTrack.Models;

namespace VehiTrack.Repositories
{
    public class FuelTypeRepository
    {
        private readonly AppContext _ctx;

        public FuelTypeRepository()
        {
            _ctx = new AppContext();
        }

        public async Task<FuelType> CreateFuelTypeAsync(FuelType fuelType)
        {
            _ctx.FuelTypes.Add(fuelType);
            await _ctx.SaveChangesAsync();
            return fuelType;
        }

        public async Task<FuelType> UpdateFuelTypeAsync(FuelType fuelType)
        {
            _ctx.FuelTypes.Update(fuelType);
            await _ctx.SaveChangesAsync();
            return fuelType;
        }

        public async Task DeleteFuelTypeAsync(FuelType fuelType)
        {
            _ctx.FuelTypes.Remove(fuelType);
            await _ctx.SaveChangesAsync();
        }

        public async Task<ICollection<FuelType>> GetFuelTypesAsync()
        {
            var fuelTypes = await _ctx.FuelTypes.ToListAsync();
            return fuelTypes;
        }
    }
}
