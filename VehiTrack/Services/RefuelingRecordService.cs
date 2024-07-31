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

        public async Task<List<IEnumerable>> GetExtendedRefuelingRecordsByVehicleId(int vehicleId)
        {
            var refuelingRecords =
                await _refuelingRecordsRepository.GetRefuelingRecordsByVehicleIdAsync(vehicleId);
            List<IEnumerable> extendedRefuelingRecords = new();
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

                    IEnumerable refuelingRecordExtended =
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

        public async Task<StatisticData> GetStatisticalDataOnConsumptionAndSupplyCosts(
            int idVehicle
        )
        {
            var refuelingRecords =
                await _refuelingRecordsRepository.GetRefuelingRecordsByVehicleIdAsync(idVehicle);
            var refuelingRecordsExtend = await GetExtendedRefuelingRecordsByVehicleId(idVehicle);
            //Total quantity of supplies (by type of fuel)
            var totalSupplies = refuelingRecords
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new { Type = g.Key, Total = g.Count() });

            //Total quantity of supplies per year (by type of fuel)
            var totalSuppliesPerYear = refuelingRecords
                .GroupBy(r => new { r.FuelType.Name, r.Date.Year })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Year = g.Key.Year,
                    Total = g.Count()
                });

            //Total quantity of supplies per month/year (by type of fuel)
            var totalSuppliesPerMonthYear = refuelingRecords
                .GroupBy(r => new
                {
                    r.FuelType.Name,
                    r.Date.Month,
                    r.Date.Year
                })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = g.Count()
                });

            //Total quantity of liters (by type of fuel)
            var totalLiters = refuelingRecords
                .GroupBy(r => r.FuelType.Name)
                .Select(r => new
                {
                    Type = r.Key,
                    TotalLiters = Math.Round(r.Sum(r => r.Quantity), 2)
                });

            //Total quantity of liters per year (by type of fuel)
            var totalLitersPerYear = refuelingRecords
                .GroupBy(r => new { r.FuelType.Name, r.Date.Year })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Year = g.Key.Year,
                    Total = Math.Round(g.Sum(r => r.Quantity), 2)
                });

            //Total quantity of liters per month/year (by type of fuel)
            var totalLitersPerMonthYear = refuelingRecords
                .GroupBy(r => new
                {
                    r.FuelType.Name,
                    r.Date.Month,
                    r.Date.Year
                })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = Math.Round(g.Sum(r => r.Quantity), 2)
                });

            //Average consumption (by type of fuel)
            var averageConsumption = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new
                {
                    Type = g.Key,
                    Average = Math.Round(g.Average(r => r.Consumption), 2)
                });

            //Average consumption per year (by type of fuel)
            var averageConsumptionPerYear = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => new { r.FuelType.Name, r.Date.Year })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Year = g.Key.Year,
                    Average = Math.Round(g.Average(r => r.Consumption), 2)
                });

            //Average consumption per month/year (by type of fuel)
            var averageConsumptionPerMonthYear = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => new
                {
                    r.FuelType.Name,
                    r.Date.Month,
                    r.Date.Year
                })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Average = Math.Round(g.Average(r => r.Consumption), 2)
                });

            //Better consumption (by type of fuel)
            var bestConsumption = refuelingRecordsExtend
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new { Type = g.Key, BestConsumption = g.Max(r => r.Consumption) });

            //Better consumption per year (by type of fuel)
            var bestConsumptionPerYear = refuelingRecordsExtend
                .GroupBy(r => new { r.FuelType.Name, r.Date.Year })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Year = g.Key.Year,
                    BestConsumption = g.Max(r => r.Consumption)
                });

            //Better consumption per month/year (by type of fuel)
            var bestConsumptionPerMonthYear = refuelingRecordsExtend
                .GroupBy(r => new
                {
                    r.FuelType.Name,
                    r.Date.Month,
                    r.Date.Year
                })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    BestConsumption = g.Max(r => r.Consumption)
                });

            //Worst consumption (by type of fuel)
            var worstConsumption = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => r.FuelType.Name)
                .Select(g => new { Type = g.Key, WorstConsumption = g.Min(r => r.Consumption) });

            //Worst consumption per year (by type of fuel)
            var worstConsumptionPerYear = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => new { r.FuelType.Name, r.Date.Year })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Year = g.Key.Year,
                    WorstConsumption = g.Min(r => r.Consumption)
                });

            //Worst consumption per month/year (by type of fuel)
            var worstConsumptionPerMonthYear = refuelingRecordsExtend
                .Where(r => r.Consumption > 0)
                .GroupBy(r => new
                {
                    r.FuelType.Name,
                    r.Date.Month,
                    r.Date.Year
                })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    WorstConsumption = g.Min(r => r.Consumption)
                });

            //Fueling costs per month/year (by type of fuel)
            var fuelingCostsPerMonthYear = refuelingRecords
                .GroupBy(r => new
                {
                    r.Date.Month,
                    r.Date.Year,
                    r.FuelType.Name
                })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Type = g.Key.Name,
                    Total = Math.Round(g.Sum(r => r.TotalCost), 2)
                });

            //Fueling costs per year (by type of fuel)
            var fuelingCostsPerYear = refuelingRecords
                .GroupBy(r => new { r.Date.Year, r.FuelType.Name })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Total = Math.Round(g.Sum(r => r.TotalCost), 2)
                });

            StatisticData statisticData =
                new()
                {
                    TotalSupplies = totalSupplies,
                    TotalSuppliesPerYear = totalSuppliesPerYear,
                    TotalSuppliesPerMonthYear = totalSuppliesPerMonthYear,
                    TotalLiters = totalLiters,
                    TotalLitersPerYear = totalLitersPerYear,
                    TotalLitersPerMonthYear = totalLitersPerMonthYear,
                    AverageConsumption = averageConsumption,
                    AverageConsumptionPerYear = averageConsumptionPerYear,
                    AverageConsumptionPerMonthYear = averageConsumptionPerMonthYear,
                    BetterConsumption = bestConsumption,
                    BetterConsumptionPerYear = bestConsumptionPerYear,
                    BetterConsumptionPerMonthYear = bestConsumptionPerMonthYear,
                    WorstConsumption = worstConsumption,
                    WorstConsumptionPerYear = worstConsumptionPerYear,
                    WorstConsumptionPerMonthYear = worstConsumptionPerMonthYear,
                    FuelingCostsPerMonthYear = fuelingCostsPerMonthYear,
                    FuelingCostsPerYear = fuelingCostsPerYear
                };
            return statisticData;
        }
    }

    public class IEnumerable : RefuelingRecord
    {
        public double Consumption { get; set; }
    }

    public class StatisticData
    {
        public IEnumerable<object> TotalSupplies { get; set; } = null!;
        public IEnumerable<object> TotalSuppliesPerYear { get; set; } = null!;
        public IEnumerable<object> TotalSuppliesPerMonthYear { get; set; } = null!;
        public IEnumerable<object> TotalLiters { get; set; } = null!;
        public IEnumerable<object> TotalLitersPerYear { get; set; } = null!;
        public IEnumerable<object> TotalLitersPerMonthYear { get; set; } = null!;
        public IEnumerable<object> AverageConsumption { get; set; } = null!;
        public IEnumerable<object> AverageConsumptionPerYear { get; set; } = null!;
        public IEnumerable<object> AverageConsumptionPerMonthYear { get; set; } = null!;
        public IEnumerable<object> BetterConsumption { get; set; } = null!;
        public IEnumerable<object> BetterConsumptionPerYear { get; set; } = null!;
        public IEnumerable<object> BetterConsumptionPerMonthYear { get; set; } = null!;
        public IEnumerable<object> WorstConsumption { get; set; } = null!;
        public IEnumerable<object> WorstConsumptionPerYear { get; set; } = null!;
        public IEnumerable<object> WorstConsumptionPerMonthYear { get; set; } = null!;
        public IEnumerable<object> FuelingCostsPerMonthYear { get; set; } = null!;
        public IEnumerable<object> FuelingCostsPerYear { get; set; } = null!;
    }
}
