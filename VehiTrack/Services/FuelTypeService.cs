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
            var createdFuelType = await _fuelTypeRepository.CreateFuelTypeAsync(fuelType);
            return createdFuelType;
        }

        public async Task<FuelType> UpdateFuelTypeAsync(FuelType fuelType)
        {
            var updatedFuelType = await _fuelTypeRepository.UpdateFuelTypeAsync(fuelType);
            return updatedFuelType;
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

        public async Task<FuelType?> GetFuelTypeByIdAsync(int id)
        {
            var fuelType = await _fuelTypeRepository.GetFuelTypeByIdAsync(id);
            return fuelType;
        }
    }
}
