using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<XRS_BedSpacePlan>> GetBedSpacePlans(int companyId)
        {
            return await _context.BedSpacePlans
                .Where(bsp => bsp.companyID == companyId)
                .ToListAsync();
        }

        public async Task<PagedResultDto<XRS_BedSpacePlan>> GetBedSpacePlanByCompanyId(int companyId,string? search = null, int pageNumber = 1,int pageSize = 10)
        {
            var query = _context.BedSpacePlans
                .Where(bsp => bsp.companyID == companyId)
                .AsQueryable();

   
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(bsp => bsp.planName.ToLower().Contains(lowerSearch));
            }

            var totalRecords = await query.CountAsync();

 
            var items = await query
                .OrderBy(bsp => bsp.planName) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new XRS_BedSpacePlan
                {
                    bedPlanID = u.bedPlanID,
                    planName = u.planName,
                    calculateAdjustRent = u.calculateAdjustRent,
                    consumedDays = u.consumedDays,
                    calculatedays = u.calculatedays,
                    calculatePartialRent = u.calculatePartialRent,
                    companyID = u.companyID,
                    isActive = u.isActive,
                    includeMess = u.includeMess,
                    messCharge = u.messCharge,
                    messChargeDays = u.messChargeDays,
                    enableMess = u.enableMess,
                })
                .ToListAsync();

            return new PagedResultDto<XRS_BedSpacePlan>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<XRS_BedSpacePlan> GetBedSpacePlanById(int bedSpacePlanId)
        {
            return await _context.BedSpacePlans
                .FirstOrDefaultAsync(u => u.bedPlanID == bedSpacePlanId);
        }

        public async Task<XRS_BedSpacePlan> CreateBedSpacePlan(XRS_BedSpacePlan createBedSpacePlan)
        {

            var bedSpace = new Models.XRS_BedSpacePlan
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

        public async Task<bool> UpdateBedSpacePlan(int id, XRS_BedSpacePlan bedSpacePlan)
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

        public async Task<bool> DeleteBedSpacePlan(int id)
        {
            var bedspacesettings = await _context.BedSpacePlans.FirstOrDefaultAsync(u => u.bedPlanID == id);
            if (bedspacesettings == null) return false;
           bedspacesettings.isActive=false;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
