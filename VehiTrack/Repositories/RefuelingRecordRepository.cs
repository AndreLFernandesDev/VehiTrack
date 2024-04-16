using Microsoft.EntityFrameworkCore;
using VehiTrack.Models;

namespace VehiTrack.Services
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

        public async Task<ICollection<RefuelingRecord>> GetRefuelingRecords()
        {
            var refuelingRecords = await _ctx.RefuelingRecords.ToListAsync();
            return refuelingRecords;
        }
    }
}
