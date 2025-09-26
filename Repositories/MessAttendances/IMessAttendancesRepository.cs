﻿using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.MessDetails
{
    public interface IMessAttendancesRepository
    {
        Task<List<TenantMessAttendanceDto>> GetMonthlyMessAttendanceAsync(int companyId, int month, int year, int? propertyId = null, int? unitId = null, int? bedSpaceid = null, string? search = null);

        Task<bool> MarkAttendanceAsync(MarkAttendanceDto dto);


    }
}
