using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.MessDetails
{
    public interface IMessAttendancesRepository
    {
        Task<List<TenantMessAttendanceDto>> GetMonthlyMessAttendanceAsync(int companyId, int propertyId, int unitId, int month, int year);

        Task<bool> MarkAttendanceAsync(MarkAttendanceDto dto);


    }
}
