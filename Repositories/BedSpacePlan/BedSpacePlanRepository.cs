using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.Controllers;
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

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            var result = await (from plan in _context.BedSpacePlans
                                join mapping in _context.BedspacePlanMessMappings
                                    on plan.bedPlanID equals mapping.bedPlanID into pm
                                from mapping in pm.DefaultIfEmpty()
                                join mess in _context.MessTypes
                                    on mapping.messID equals mess.messID into mm
                                from mess in mm.DefaultIfEmpty()
                                select new
                                {
                                    plan.bedPlanID,
                                    plan.companyID,
                                    plan.planName,
                                    plan.enableTax,
                                    plan.calculatedays,
                                    plan.enableAdjustRent,
                                    plan.calculateAdjustRent,
                                    plan.calculatePartialRent,
                                    plan.enableMess,
                                    plan.messCharge,
                                    plan.includeMess,
                                    plan.messChargeDays,
                                    plan.consumedDays,
                                    plan.isActive,
                                    bpmID = mapping != null ? mapping.bpmID : 0,
                                    messID = mapping != null ? mapping.messID : 0,
                                    messTypeName = mess != null ? mess.MessName : string.Empty,
                                    active = mapping != null ? mapping.active : false
                                }).ToListAsync();

            return result;
        }

   
        public async Task<object?> GetByIdAsync(int bedPlanID)
        {
            var result = await (from plan in _context.BedSpacePlans
                                where plan.bedPlanID == bedPlanID
                                join mapping in _context.BedspacePlanMessMappings
                                    on plan.bedPlanID equals mapping.bedPlanID into pm
                                from mapping in pm.DefaultIfEmpty()
                                join mess in _context.MessTypes
                                    on mapping.messID equals mess.messID into mm
                                from mess in mm.DefaultIfEmpty()
                                select new
                                {
                                    plan.bedPlanID,
                                    plan.companyID,
                                    plan.planName,
                                    plan.enableTax,
                                    plan.calculatedays,
                                    plan.enableAdjustRent,
                                    plan.calculateAdjustRent,
                                    plan.calculatePartialRent,
                                    plan.enableMess,
                                    plan.messCharge,
                                    plan.includeMess,
                                    plan.messChargeDays,
                                    plan.consumedDays,
                                    plan.isActive,
                                    bpmID = mapping != null ? mapping.bpmID : 0,
                                    messID = mapping != null ? mapping.messID : 0,
                                    messTypeName = mess != null ? mess.MessName : string.Empty,
                                    active = mapping != null ? mapping.active : false
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetByCompanyIdAsync(int companyID)
        {
            var result = await (from plan in _context.BedSpacePlans
                                where plan.companyID == companyID
                                join mapping in _context.BedspacePlanMessMappings
                                    on plan.bedPlanID equals mapping.bedPlanID into pm
                                from mapping in pm.DefaultIfEmpty()
                                join mess in _context.MessTypes
                                    on mapping.messID equals mess.messID into mm
                                from mess in mm.DefaultIfEmpty()
                                select new
                                {
                                    plan.bedPlanID,
                                    plan.companyID,
                                    plan.planName,
                                    plan.enableTax,
                                    plan.calculatedays,
                                    plan.enableAdjustRent,
                                    plan.calculateAdjustRent,
                                    plan.calculatePartialRent,
                                    plan.enableMess,
                                    plan.messCharge,
                                    plan.includeMess,
                                    plan.messChargeDays,
                                    plan.consumedDays,
                                    plan.isActive,
                                    bpmID = mapping != null ? mapping.bpmID : 0,
                                    messID = mapping != null ? mapping.messID : 0,
                                    messTypeName = mess != null ? mess.MessName : string.Empty,
                                    active = mapping != null ? mapping.active : false
                                }).ToListAsync();

            return result;
        }
        public async Task<XRS_BedSpacePlan> CreateAsync(XRS_BedSpacePlan entity)
        {
            _context.BedSpacePlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(XRS_BedSpacePlan entity)
        {
            var existing = await _context.BedSpacePlans.FindAsync(entity.bedPlanID);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int bedID)
        {
            var entity = await _context.BedSpacePlans.FindAsync(bedID);
            if (entity == null) return false;

            _context.BedSpacePlans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
