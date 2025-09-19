using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.BedSpacePlan
{
    public class BedSpacePlanRepository:IBedSpacePlanRepository
    {
        private readonly ApplicationDbContext _context;
        public BedSpacePlanRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.BedSpacePlan>> GetBedSpacePlans()
        {

            return await _context.BedSpacePlans
                 .ToListAsync();

        }


        public async Task<PagedResultDto<Models.BedSpacePlan>> GetBedSpacePlanbyCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.BedSpacePlans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
           .Select(u => new Models.BedSpacePlan
           {
               bedPlanID = u.bedPlanID,
               planName = u.planName,
               calculateAdjustRent = u.calculateAdjustRent,
               consumedDays = u.consumedDays,
               calculatedays = u.calculatedays,
               calculatePartialRent = u.calculatePartialRent,
               companyID = u.companyID,
               isActive = u.isActive,
               includeMess=u.includeMess,
               messCharge=u.messCharge,
               messChargeDays = u.messChargeDays,
               enableMess=u.enableMess,
           })
                .ToListAsync();
                    return new PagedResultDto<Models.BedSpacePlan>
                 {
                     Items = items,
                     PageNumber = pageNumber,
                     PageSize = pageSize,
                     TotalRecords = totalRecords
                 };

        }

        public async Task<IEnumerable<Models.BedSpacePlan>> GetBedSpacePlanbyId(int bedSpacePlanId)
        {

            return await _context.BedSpacePlans
                .Where(u => u.bedPlanID == bedSpacePlanId)
                 .ToListAsync();

        }

        public async Task<Models.BedSpacePlan> CreateBedSpacePlan(DTOs.CreateBedSpacePlan createBedSpacePlan)
        {

            var bedSpace = new Models.BedSpacePlan
            {
                companyID = createBedSpacePlan.companyID,
                planName = createBedSpacePlan.planName,
                enableTax = createBedSpacePlan.enableTax,
                calculatedays = createBedSpacePlan.calculatedays,
                calculateAdjustRent = createBedSpacePlan.calculateAdjustRent,
                enableAdjustRent=createBedSpacePlan.enableAdjustRent,
                enableMess=createBedSpacePlan.enableMess,
                enablePartialRent=createBedSpacePlan.enablePartialRent,
                calculatePartialRent= createBedSpacePlan.calculatePartialRent,
                includeMess=createBedSpacePlan.includeMess,
                messCharge=createBedSpacePlan.messCharge,
                messChargeDays=createBedSpacePlan.messChargeDays,
                consumedDays=createBedSpacePlan.consumedDays,
                isActive=createBedSpacePlan.isActive,
            };
            await _context.BedSpacePlans.AddAsync(bedSpace);
            await _context.SaveChangesAsync();
            return bedSpace;

        }

        public async Task<bool> DeleteBedSpacePlan(int id)
        {
            var bedspacesettings = await _context.BedSpacePlans.FirstOrDefaultAsync(u => u.bedPlanID == id);
            if (bedspacesettings == null) return false;
           bedspacesettings.isActive=false;
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBedSpacePlan(int id, Models.BedSpacePlan bedSpacePlan )
        {
            var updatebedSpacePlan = await _context.BedSpacePlans.FirstOrDefaultAsync(u => u.bedPlanID == id);
            if (updatebedSpacePlan == null) return false;

            updatebedSpacePlan.planName = bedSpacePlan.planName ?? bedSpacePlan.planName;
            updatebedSpacePlan.companyID = bedSpacePlan.companyID;
            updatebedSpacePlan.includeMess = bedSpacePlan.includeMess;
            updatebedSpacePlan.enableMess = bedSpacePlan.enableMess;
            updatebedSpacePlan.calculatePartialRent = bedSpacePlan.calculatePartialRent;
            updatebedSpacePlan.calculatedays = bedSpacePlan.calculatedays;
            updatebedSpacePlan.calculateAdjustRent = bedSpacePlan.calculateAdjustRent;
            updatebedSpacePlan.consumedDays = bedSpacePlan.consumedDays;
            updatebedSpacePlan.enableAdjustRent = bedSpacePlan.enableAdjustRent;
            updatebedSpacePlan.enableTax = bedSpacePlan.enableTax;
            updatebedSpacePlan.isActive = bedSpacePlan.isActive;
            updatebedSpacePlan.enablePartialRent = bedSpacePlan.enablePartialRent;
            updatebedSpacePlan.includeMess = bedSpacePlan.includeMess;
            updatebedSpacePlan.messCharge = bedSpacePlan.messCharge;
            updatebedSpacePlan.messChargeDays = bedSpacePlan.messChargeDays;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.BedSpacePlan>> GetBedSpacePlanAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.BedSpacePlans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.planName.Contains(search)); // Adjust property as needed
                
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.BedSpacePlan
                {
                    bedPlanID=u.bedPlanID,
                    planName=u.planName,
                    calculateAdjustRent=u.calculateAdjustRent,
                    consumedDays=u.consumedDays,
                    calculatedays=u.calculatedays,
                    calculatePartialRent=u.calculatePartialRent,
                    companyID=u.companyID,
                    isActive=u.isActive,
                })
                .ToListAsync();

            return new PagedResultDto<Models.BedSpacePlan>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
