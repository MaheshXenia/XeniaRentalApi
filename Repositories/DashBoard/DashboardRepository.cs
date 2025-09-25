using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;


namespace XeniaRentalApi.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepsitory
    {

        private readonly ApplicationDbContext _context;
        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<RentDashboardDto> GetRentDashboardAsync(DateTime fromDate, DateTime toDate)
        {
            var activeAssignments = await _context.TenantAssignemnts
                .Where(t => t.rentDueDate >= fromDate && t.rentDueDate <= toDate && !t.isClosure)
                .ToListAsync();

            var vouchers = await _context.Vouchers
                .Where(v => v.VoucherType == "Pay Rent" && v.VoucherDate >= fromDate && v.VoucherDate <= toDate)
                .ToListAsync();

            int paidCount = 0;
            decimal totalPaidAmount = 0;
            int notPaidCount = 0;
            decimal totalNotPaidAmount = 0;

            foreach (var tenant in activeAssignments)
            {
                var voucher = vouchers.FirstOrDefault(v => v.DrID == tenant.tenantID && v.unitID == tenant.unitID);
                if (voucher != null)
                {
                    paidCount++;
                    totalPaidAmount += voucher.Amount;
                }
                else
                {
                    notPaidCount++;
                    totalNotPaidAmount += tenant.rentAmt;
                }
            }

            var occupiedPropertyIds = activeAssignments.Select(t => t.propID).Distinct().Count();
            var occupiedUnitIds = activeAssignments.Select(t => t.unitID).Distinct().Count();

            var totalPropertiesCount = await _context.Properties.CountAsync();
            var totalUnitsCount = await _context.Units.CountAsync();

            int vacantProperties = totalPropertiesCount - occupiedPropertyIds;
            int vacantUnits = totalUnitsCount - occupiedUnitIds;

            decimal averageRentPerProperty = occupiedPropertyIds > 0
                ? totalPaidAmount / occupiedPropertyIds
                : 0;

            int highRiskTenantCount = activeAssignments.Count(t =>
                !vouchers.Any(v => v.DrID == t.tenantID && v.unitID == t.unitID));

            int highRiskPropertyCount = activeAssignments
                .Where(t => !vouchers.Any(v => v.DrID == t.tenantID && v.unitID == t.unitID))
                .Select(t => t.propID)
                .Distinct()
                .Count();

       
            decimal occupancyRate = totalPropertiesCount > 0
                ? (decimal)occupiedPropertyIds / totalPropertiesCount * 100
                : 0;

            decimal collectionRate = (paidCount + notPaidCount) > 0
                ? (decimal)paidCount / (paidCount + notPaidCount) * 100
                : 0;

            var topPerformingProperties = vouchers
                .GroupBy(v => v.PropID)
                .Select(g => new
                {
                    PropertyId = g.Key,
                    TotalCollected = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalCollected)
                .Take(3)
                .ToList();

            int topPerformingPropertyCount = topPerformingProperties.Count;

            return new RentDashboardDto
            {
                TotalPaidCount = paidCount,
                TotalPaidAmount = totalPaidAmount,
                TotalNotPaidCount = notPaidCount,
                TotalNotPaidAmount = totalNotPaidAmount,
                TotalOccupiedProperties = occupiedPropertyIds,
                TotalOccupiedUnits = occupiedUnitIds,
                VacantProperties = vacantProperties,
                VacantUnits = vacantUnits,
                TopPerformingPropertyCount = topPerformingPropertyCount,
                AverageRentPerProperty = averageRentPerProperty,
                HighRiskTenantCount = highRiskTenantCount,
                HighRiskPropertyCount = highRiskPropertyCount,
                OccupancyRate = Math.Round(occupancyRate, 2),
                CollectionRate = Math.Round(collectionRate, 2)
            };
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRentRevenueAsync(int year)
        {
            var vouchers = await _context.Vouchers
                .Where(v => v.VoucherType == "Pay Rent" && v.VoucherDate.Year == year)
                .ToListAsync();

            var monthlyRevenue = Enumerable.Range(1, 12)
                .Select(month =>
                {
                    var total = vouchers
                        .Where(v => v.VoucherDate.Month == month)
                        .Sum(v => v.Amount);

                    return new MonthlyRevenueDto
                    {
                        Month = new DateTime(year, month, 1).ToString("MMM"),
                        TotalRent = total
                    };
                })
                .ToList();

            return monthlyRevenue;
        }




    }
}
