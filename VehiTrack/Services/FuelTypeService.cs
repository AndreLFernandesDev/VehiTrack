using VehiTrack.Models;
using VehiTrack.Repositories;

namespace VehiTrack.Services
{
    public class FuelTypeService
    {
        private readonly FuelTypeRepository _fuelTypeRepository;

        public FuelTypeService()
        {
            _fuelTypeRepository = new FuelTypeRepository();
        }

        public async Task<FuelType> CreateFuelTypeAsync(FuelType fuelType)
        {
            var createFuelType = await _fuelTypeRepository.CreateFuelTypeAsync(fuelType);
            return createFuelType;
        }

        public async Task<FuelType> UpdateFuelTypeAsync(FuelType fuelType)
        {
            var updateFuelType = await _fuelTypeRepository.UpdateFuelTypeAsync(fuelType);
            return updateFuelType;
        }

        public async Task DeleteFuelTypeAsync(FuelType fuelType)
        {
            await _fuelTypeRepository.DeleteFuelTypeAsync(fuelType);
        }

        public async Task<ICollection<FuelType>> GetFuelTypesAsync()
        {
            var fuelTypes = await _fuelTypeRepository.GetFuelTypesAsync();
            return fuelTypes;
        }
    }
}
