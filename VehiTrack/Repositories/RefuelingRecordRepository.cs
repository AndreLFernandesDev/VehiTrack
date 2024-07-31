using Microsoft.EntityFrameworkCore;
using VehiTrack.Models;

namespace VehiTrack.Repositories
{
    public class RefuelingRecordsRepository
    {
        private readonly AppContext _ctx;

        public RefuelingRecordsRepository()
        {
            _ctx = new AppContext();
        }

        public async Task<RefuelingRecord> CreateRefuelingRecordAsync(
            RefuelingRecord refuelingRecord
        )
        {
            _ctx.RefuelingRecords.Add(refuelingRecord);
            await _ctx.SaveChangesAsync();
            return refuelingRecord;
        }

        public async Task<RefuelingRecord> UpdateRefuelingRecordAsync(
            RefuelingRecord refuelingRecord
        )
        {
            _ctx.RefuelingRecords.Update(refuelingRecord);
            await _ctx.SaveChangesAsync();
            return refuelingRecord;
        }

        public async Task DeleteRefuelingRecordAsync(RefuelingRecord refuelingRecord)
        {
            _ctx.RefuelingRecords.Remove(refuelingRecord);
            await _ctx.SaveChangesAsync();
        }

        public async Task<ICollection<RefuelingRecord>> GetRefuelingRecordsAsync()
        {
            var refuelingRecords = await _ctx.RefuelingRecords.ToListAsync();
            return refuelingRecords;
        }

        public async Task<RefuelingRecord?> GetRefuelingRecordByIdAsync(int id)
        {
            var refuelingRecord = await _ctx.RefuelingRecords.FindAsync(id);
            return refuelingRecord;
        }

        public async Task<ICollection<RefuelingRecord>> GetRefuelingRecordsByVehicleIdAsync(
            int vehicleId
        )
        {
            var refuelingRecords = await _ctx
                .RefuelingRecords.Where(v => v.VehicleId == vehicleId)
                .Include(v => v.FuelType)
                .OrderBy(r => r.Date)
                .ToListAsync();
            return refuelingRecords;
        }
    }
}
