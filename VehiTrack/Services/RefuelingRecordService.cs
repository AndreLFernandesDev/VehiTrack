using Microsoft.EntityFrameworkCore.Query.Internal;
using VehiTrack.Models;
using VehiTrack.Repositories;

namespace VehiTrack.Services
{
    public class RefuelingRecordService
    {
        private readonly RefuelingRecordsRepository _refuelingRecordsRepository;

        public RefuelingRecordService()
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

        public async Task<List<RefuelingRecordExtended>> GetExtendedRefuelingRecordsByVehicleId(
            int vehicleId
        )
        {
            var refuelingRecords =
                await _refuelingRecordsRepository.GetRefuelingRecordsByVehicleIdAsync(vehicleId);
            List<RefuelingRecordExtended> extendedRefuelingRecords = new();
            if (refuelingRecords.Count > 1)
            {
                for (int i = 0; i <= refuelingRecords.Count - 1; i++)
                {
                    RefuelingRecord refuelingRecord;
                    RefuelingRecord refuelingRecordNext;
                    double currentOdometer;
                    double previousOdometer;
                    double quantityOfLiters;
                    refuelingRecord = refuelingRecords.ElementAt(i);
                    if (i == refuelingRecords.Count - 1)
                    {
                        break;
                    }
                    else
                    {
                        refuelingRecordNext = refuelingRecords.ElementAt(i + 1);
                        currentOdometer = refuelingRecordNext.OdometerCounter;
                        previousOdometer = refuelingRecord.OdometerCounter;
                        quantityOfLiters = refuelingRecordNext.Quantity;
                    }

                    double consumption = 0;
                    if (refuelingRecord.IsFull == true && refuelingRecordNext.IsFull == true)
                    {
                        if (previousOdometer > currentOdometer)
                        {
                            double maximumOdometer;
                            if (previousOdometer > 99999)
                            {
                                maximumOdometer = 999999;
                            }
                            else
                            {
                                maximumOdometer = 99999;
                            }
                            consumption = Math.Round(
                                (maximumOdometer - previousOdometer + currentOdometer)
                                    / quantityOfLiters,
                                2
                            );
                        }
                        else
                        {
                            consumption = Math.Round(
                                (currentOdometer - previousOdometer) / quantityOfLiters,
                                2
                            );
                        }
                    }

                    RefuelingRecordExtended refuelingRecordExtended =
                        new()
                        {
                            Id = refuelingRecord.Id,
                            Date = refuelingRecord.Date,
                            OdometerCounter = refuelingRecord.OdometerCounter,
                            IsFull = refuelingRecord.IsFull,
                            Quantity = refuelingRecord.Quantity,
                            UnitPrice = refuelingRecord.UnitPrice,
                            FuelTypeId = refuelingRecord.FuelTypeId,
                            FuelType = refuelingRecord.FuelType,
                            VehicleId = refuelingRecord.VehicleId,
                            Vehicle = refuelingRecord.Vehicle,
                            TotalCost = refuelingRecord.TotalCost,
                            Consumption = consumption
                        };
                    extendedRefuelingRecords.Add(refuelingRecordExtended);
                }
            }
            return extendedRefuelingRecords;
        }

        public async Task GetStatisticalDataOnConsumptionAndSupplyCosts(int idVehicle)
        {
            var refuelingRecords =
                await _refuelingRecordsRepository.GetRefuelingRecordsByVehicleIdAsync(idVehicle);
            //Total quantity of supplies (by type of fuel)
            var totalSupplies = refuelingRecords
                .GroupBy(r => r.FuelType.Name)
                .Select(r => new { Type = r.Key, Total = r.Count() });

            //Total quantity of liters (by type of fuel)
            var totalLiters = refuelingRecords
                .GroupBy(r => r.FuelType.Name)
                .Select(r => new
                {
                    Type = r.Key,
                    TotalLyters = Math.Round(r.Sum(r => r.Quantity), 2)
                });

            //Average consumption (by type of fuel)
            var refuelingRecordsExtend = await GetExtendedRefuelingRecordsByVehicleId(idVehicle);
            var overallAverage = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new
                {
                    Type = g.Key,
                    Average = Math.Round(g.Sum(r => r.Consumption) / g.Count(), 2)
                });

            //Better consumption (by type of fuel)
            var betterConsumption = refuelingRecordsExtend
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new { Type = g.Key, BetterConsumption = g.Max(g => g.Consumption) });

            //Worst consumption (by type of fuel)
            var worstConsumption = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new { Type = g.Key, WorstConsumption = g.Min(g => g.Consumption) });

            foreach (var item in worstConsumption)
            {
                Console.WriteLine("Tipo {0}={1}", item.Type, item.WorstConsumption);
            }
        }
    }

    public class RefuelingRecordExtended : RefuelingRecord
    {
        public double Consumption { get; set; }
    }
}
