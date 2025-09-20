using Microsoft.EntityFrameworkCore;
using Stripe;
using System.ComponentModel.Design;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.TenantAssignment
{
    public class TenantAssignmentRepository : ITenantAssignmentRepository
    {
        private readonly ApplicationDbContext _context;
        public TenantAssignmentRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<TenantAssignmentGetDto>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.TenantAssignemnts
                .Include(t => t.Properties)
                .Include(t => t.Tenant)
                .Include(t => t.Unit)
                .Where(t => t.companyID == companyId)
                .Select(t => new TenantAssignmentGetDto
                {
                    tenantAssignId = t.tenantAssignId,
                    propID = t.propID,
                    PropName = t.Properties.propertyName,
                    unitID = t.unitID,
                    UnitName = t.Unit.UnitName,
                    tenantID = t.tenantID,
                    TenantName = t.Tenant.tenantName,
                    TenantContactNo = t.Tenant.phoneNumber,
                    rentAmt = t.rentAmt,
                    rentConcession = t.rentConcession,
                    messConcession = t.messConcession,
                    agreementStartDate = t.agreementStartDate,
                    agreementEndDate = t.agreementEndDate,
                    isActive = t.isActive,
                    isClosure = t.isClosure,
                    notes = t.notes
                })
                .ToListAsync();
        }

        public async Task<TenantAssignmentGetDto?> GetByIdAsync(int tenantAssignId)
        {
            return await _context.TenantAssignemnts
                .Include(t => t.Properties)
                .Include(t => t.Tenant)
                .Include(t => t.Unit)
                .Where(t => t.tenantAssignId == tenantAssignId)
                .Select(t => new TenantAssignmentGetDto
                {
                    tenantAssignId = t.tenantAssignId,
                    propID = t.propID,
                    PropName = t.Properties.propertyName,
                    unitID = t.unitID,
                    UnitName = t.Unit.UnitName,
                    tenantID = t.tenantID,
                    TenantName = t.Tenant.tenantName,
                    TenantContactNo = t.Tenant.phoneNumber,
                    rentAmt = t.rentAmt,
                    rentConcession = t.rentConcession,
                    messConcession = t.messConcession,
                    agreementStartDate = t.agreementStartDate,
                    agreementEndDate = t.agreementEndDate,
                    isActive = t.isActive,
                    isClosure = t.isClosure,
                    notes = t.notes
                })
                .FirstOrDefaultAsync();
        }

        public async Task<XRS_TenantAssignment> CreateAsync(TenantAssignmentCreateDto dto)
        {
            var entity = new XRS_TenantAssignment
            {
                propID = dto.propID,
                unitID = dto.unitID,
                tenantID = dto.tenantID,
                companyID = dto.companyID,
                securityAmt = dto.securityAmt,
                isTaxable = dto.isTaxable,
                bedSpaceID = dto.bedSpaceID,
                rentAmt = dto.rentAmt,
                rentConcession = dto.rentConcession,
                messConcession = dto.messConcession,
                frequency = dto.frequency,
                collectionType = dto.collectionType,
                agreementStartDate = dto.agreementStartDate,
                agreementEndDate = dto.agreementEndDate,
                rentCollection = dto.rentCollection,
                escalationPer = dto.escalationPer,
                nextescalationDate = dto.nextescalationDate,
                rentDueDate = dto.rentDueDate,
                notes = dto.notes,
                isActive = true
            };

            _context.TenantAssignemnts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<XRS_TenantAssignment?> UpdateAsync(TenantAssignmentCreateDto dto)
        {
            var entity = await _context.TenantAssignemnts.FindAsync(1);
            if (entity == null) return null;

            entity.securityAmt = dto.securityAmt;
            entity.isTaxable = dto.isTaxable;
            entity.rentAmt = dto.rentAmt;
            entity.rentConcession = dto.rentConcession;
            entity.messConcession = dto.messConcession;
            entity.frequency = dto.frequency;
            entity.collectionType = dto.collectionType;
            entity.agreementStartDate = dto.agreementStartDate;
            entity.agreementEndDate = dto.agreementEndDate;
            entity.rentCollection = dto.rentCollection;
            entity.escalationPer = dto.escalationPer;
            entity.nextescalationDate = dto.nextescalationDate;
            entity.rentDueDate = dto.rentDueDate;
            entity.notes = dto.notes;
           /* entity.isActive = dto.isa;
            entity.isClosure = dto.isClosure;
            entity.closureReason = dto.closureReason;
            entity.closureDate = dto.closureDate;
            entity.refundAmount = dto.refundAmount;*/

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int tenantAssignId)
        {
            var entity = await _context.TenantAssignemnts.FindAsync(tenantAssignId);
            if (entity == null) return false;

            _context.TenantAssignemnts.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }


} 

