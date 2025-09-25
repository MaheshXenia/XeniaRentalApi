using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.MessDetails
{
    public class MessAttendancesRepository: IMessAttendancesRepository
    {
        private readonly ApplicationDbContext _context;
        public MessAttendancesRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<List<TenantMessAttendanceDto>> GetMonthlyMessAttendanceAsync(int companyId, int propertyId, int unitId, int month, int year)
        {
            var tenants = await _context.TenantAssignemnts
                .Include(t => t.Tenant)
                .Include(t => t.BedSpace)
                .Where(t => t.companyID == companyId &&
                            t.propID == propertyId &&
                            t.unitID == unitId &&
                            !t.isClosure)
                .ToListAsync();

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var result = new List<TenantMessAttendanceDto>();

            foreach (var tenant in tenants)
            {
                if (tenant.BedSpace?.planID == null) continue;

                var messTypes = await _context.BedspacePlanMessMappings
                    .Where(m => m.bedPlanID == tenant.BedSpace.planID)
                    .Join(_context.MessTypes,
                          mapping => mapping.messID,
                          type => type.messID,
                          (mapping, type) => new
                          {
                              mapping.messID,
                              type.MessName,
                              type.MessCode,
                          })
                    .ToListAsync();

               
                var attendanceRecords = await _context.MessAttendances
                    .Where(a => a.TenantAssignID == tenant.tenantAssignId &&
                                a.AttendanceDate.Month == month &&
                                a.AttendanceDate.Year == year)
                    .ToListAsync();

                var tenantDto = new TenantMessAttendanceDto
                {
                    TenantId = tenant.tenantID,
                    TenantAssignId = tenant.tenantAssignId,
                    TenantName = tenant.Tenant?.tenantName ?? string.Empty,
                    MessTypes = new List<MessTypeAttendanceDto>()
                };

                foreach (var mess in messTypes)
                {
                    var messTypeDto = new MessTypeAttendanceDto
                    {
                        MessTypeId = mess.messID,
                        MessName = mess.MessName,
                        MessCode = mess.MessCode,

                    };

                    for (int day = 1; day <= daysInMonth; day++)
                    {
                        var date = new DateTime(year, month, day);
                        var record = attendanceRecords.FirstOrDefault(a =>
                            a.MessTypeID == mess.messID &&
                            a.AttendanceDate.Date == date.Date);

                        messTypeDto.Days.Add(new MessAttendanceDayDto
                        {
                            Day = day,
                            IsPresent = record?.IsPresent ?? false
                        });
                    }

                    tenantDto.MessTypes.Add(messTypeDto);
                }

                result.Add(tenantDto);
            }

            return result;
        }

        public async Task<bool> MarkAttendanceAsync(MarkAttendanceDto dto)
        {
            var existing = await _context.MessAttendances
                .FirstOrDefaultAsync(a => a.TenantAssignID == dto.TenantAssignId &&
                                          a.MessTypeID == dto.MessTypeId &&
                                          a.AttendanceDate.Date == dto.AttendanceDate.Date);

            if (existing != null)
            {
                existing.IsPresent = dto.IsPresent;
                existing.CreatedBy = dto.CreatedBy;
                existing.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                var newAttendance = new XRS_MessAttendance
                {
                    TenantID = dto.TenantId,
                    TenantAssignID = dto.TenantAssignId,
                    BedSpacePlanID = dto.BedSpacePlanId,
                    MessTypeID = dto.MessTypeId,
                    AttendanceDate = dto.AttendanceDate.Date,
                    IsPresent = dto.IsPresent,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                _context.MessAttendances.Add(newAttendance);
            }

            await _context.SaveChangesAsync();
            return true;
        }



    }
}
