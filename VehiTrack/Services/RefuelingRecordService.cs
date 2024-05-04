using System.Xml.XPath;
using VehiTrack.Models;
using VehiTrack.Repositories;

namespace VehiTrack.Services
{
    public class RefuelingRecordsService
    {
        private readonly RefuelingRecordsRepository _refuelingRecordsRepository;

        public RefuelingRecordsService()
        {
            _refuelingRecordsRepository = new RefuelingRecordsRepository();
        }

        public async Task<RefuelingRecord> CreateRefuelingRecordAsync(
            RefuelingRecord refuelingRecord
        )
        {
            var createdRefuelingRecord =
                await _refuelingRecordsRepository.CreateRefuelingRecordAsync(refuelingRecord);
            return createdRefuelingRecord;
        }

        public async Task<RefuelingRecord> UpdateRefuelingRecordAsync(
            RefuelingRecord refuelingRecord
        )
        {
            var updatedRefuelingRecord =
                await _refuelingRecordsRepository.UpdateRefuelingRecordAsync(refuelingRecord);
            return updatedRefuelingRecord;
        }

        public async Task DeleteRefuelingRecordAsync(RefuelingRecord refuelingRecord)
        {
            await _refuelingRecordsRepository.DeleteRefuelingRecordAsync(refuelingRecord);
        }

        public async Task<ICollection<RefuelingRecord>> GetRefuelingRecords()
        {
            var refuelingRecords = await _refuelingRecordsRepository.GetRefuelingRecordsAsync();
            return refuelingRecords;
        }

        public async Task<RefuelingRecord?> GetRefuelingRecordByIdAsync(int id)
        {
            var refuelingRecord = await _refuelingRecordsRepository.GetRefuelingRecordByIdAsync(id);
            return refuelingRecord;
        }

        public async Task<List<RefuelingRecordExtend>> CalculateConsumptionAsync(int id)
        {
            var refuelingRecords =
                await _refuelingRecordsRepository.GetRefuelingRecordsByVehicleIdAsync(id);
            List<RefuelingRecordExtend> refuelingRecordExtends = new();
            if (refuelingRecords.Count > 1)
            {
                for (int i = 0; i < refuelingRecords.Count - 1; i++)
                {
                    RefuelingRecord refuelingRecord = null!;
                    RefuelingRecord refuelingRecordPrevius = null!;
                    double previousOdometer = 0;
                    double currentOdometer = 0;
                    double quantityOfLiters = 0;
                    if (i >= 1)
                    {
                        refuelingRecord = refuelingRecords.ElementAt(i);
                        refuelingRecordPrevius = refuelingRecords.ElementAt(i - 1);
                        previousOdometer = refuelingRecordPrevius.OdometerCounter;
                        currentOdometer = refuelingRecord.OdometerCounter;
                        quantityOfLiters = refuelingRecord.Quantity;
                    }
                    else if (i == 0)
                    {
                        refuelingRecord = refuelingRecords.ElementAt(i);
                        refuelingRecordPrevius = refuelingRecords.ElementAt(i);
                        previousOdometer = refuelingRecordPrevius.OdometerCounter;
                        currentOdometer = refuelingRecord.OdometerCounter;
                        quantityOfLiters = refuelingRecord.Quantity;
                    }

                    double consumption = 0;
                    if (refuelingRecord.IsFull == true && refuelingRecordPrevius.IsFull == true)
                    {
                        consumption = Math.Round(
                            (currentOdometer - previousOdometer) / quantityOfLiters,
                            2
                        );
                    }

                    RefuelingRecordExtend refuelingRecordExtend =
                        new()
                        {
                            Id = refuelingRecord.Id,
                            DateTime = refuelingRecord.DateTime,
                            OdometerCounter = refuelingRecord.OdometerCounter,
                            IsFull = refuelingRecord.IsFull,
                            Quantity = refuelingRecord.Quantity,
                            UnitPrice = refuelingRecord.UnitPrice,
                            FuelTypeId = refuelingRecord.FuelTypeId,
                            FuelType = refuelingRecord.FuelType,
                            VehicleId = refuelingRecord.VehicleId,
                            Vehicle = refuelingRecord.Vehicle,
                            Consumption = consumption
                        };
                    refuelingRecordExtends.Add(refuelingRecordExtend);
                }
            }
            return refuelingRecordExtends;
        }
    }

    public class RefuelingRecordExtend : RefuelingRecord
    {
        public double Consumption { get; set; }
    }
}
