using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Dashboard
{
    public interface IDashboardRepsitory
    {
        Task<RentDashboardDto> GetRentDashboardAsync(DateTime fromDate, DateTime toDate);
        Task<List<MonthlyRevenueDto>> GetMonthlyRentRevenueAsync(int year);
    }
}
