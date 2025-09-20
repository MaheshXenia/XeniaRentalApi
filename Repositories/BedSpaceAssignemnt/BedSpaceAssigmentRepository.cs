using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.BedSpaceAssignemnt
{
    public class BedSpaceAssigmentRepository:IBedSpaceAssignmentRepository
    {
        private readonly ApplicationDbContext _context;
        public BedSpaceAssigmentRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.BedSpaceAssignemnt>> GetBedSpaceAssigments()
        {

            return await _context.BedSpaceAssignemnt
                  .GroupJoin(_context.Properties,
                a => a.propID,
                 p => p.PropID,
                (a, props) => new { a, Property = props.FirstOrDefault() })
            .Join(_context.Units,
                ap => ap.a.unitID,
                u => u.UnitId,
            (ap, unit) => new { ap.a, ap.Property, Unit = unit })
            .Join(_context.Tenants,
                apu => apu.a.tenantID,
                t => t.tenantID,
                (apu, tenant) => new { apu.a, apu.Property, apu.Unit, Tenant = tenant })
              .Join(_context.BedSpaces,
                aput => aput.a.bedSpaceID,
                 b => b.bedID,
                (aput, bedSpace) => new
                 {
                    aput.a,
                    aput.Property,
                    aput.Unit,
                    aput.Tenant,
                    BedSpace = bedSpace
                })

                .Select(u => new Models.BedSpaceAssignemnt
                 {
                     assignmentID = u.a.assignmentID,
                     amount = u.a.amount,
                     agreementStartDate = u.a.agreementStartDate,
                     agreementEndDate = u.a.agreementEndDate,
                     propID = u.a.propID,
                     unitID = u.a.unitID,
                     tenantID= u.a.tenantID,
                     UnitName = u.Unit != null ? u.Unit.UnitName : null,
                     companyID = u.a.companyID,
                     rentAmt = u.a.rentAmt,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     isActive = u.a.isActive,
                     TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                     isTaxable = u.a.isTaxable,
                     taxRatePer=u.a.taxRatePer,
                     rentConcession=u.a.rentConcession,
                     messConcession=u.a.messConcession,
                     frequency=u.a.frequency,
                     rentDueDate=u.a.rentDueDate,
                     rentescalationPer=u.a.rentescalationPer,
                     charges=u.a.charges,
                     securityAmt=u.a.securityAmt,
                     bedSpaceID = u.a.bedSpaceID,
                     BedSpaceName = u.BedSpace.bedSpaceName,
                     escalationDate=u.a.escalationDate,

                 }).ToListAsync();

        }

        
        public async Task<PagedResultDto<Models.BedSpaceAssignemnt>> GetBedSpaceAssignemntsByCompanyId(int companyId, string srch,int pageNumber, int pageSize)
        {

            var query = _context.BedSpaceAssignemnt.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID.Equals(companyId)); // Adjust property as needed
            }
            if (!string.IsNullOrWhiteSpace(srch))
            {
                query = query.Where(u => u.Tenant.phoneNumber.Contains(srch));
            }

            var totalRecords = await query.CountAsync();

            var items = await query
    .GroupJoin(_context.Properties,
        a => a.propID,
        p => p.PropID,
        (a, props) => new { a, Property = props.FirstOrDefault() })
    .Join(_context.Units,
        ap => ap.a.unitID,
        u => u.UnitId,
        (ap, unit) => new { ap.a, ap.Property, Unit = unit })
    .Join(_context.Tenants,
        apu => apu.a.tenantID,
        t => t.tenantID,
        (apu, tenant) => new { apu.a, apu.Property, apu.Unit, Tenant = tenant })
      .Join(_context.BedSpaces,
                aput => aput.a.bedSpaceID,
                 b => b.bedID,
                (aput, bedSpace) => new
                {
                    aput.a,
                    aput.Property,
                    aput.Unit,
                    aput.Tenant,
                    BedSpace = bedSpace
                })
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
               .Select(u => new Models.BedSpaceAssignemnt
                {
                    assignmentID = u.a.assignmentID,
                    amount = u.a.amount,
                    agreementStartDate = u.a.agreementStartDate,
                    agreementEndDate = u.a.agreementEndDate,
                    propID = u.a.propID,
                    unitID = u.a.unitID,
                    tenantID = u.a.tenantID,
                    UnitName = u.Unit != null ? u.Unit.UnitName : null,
                    companyID = u.a.companyID,
                    rentAmt = u.a.rentAmt,
                    PropName = u.Property != null ? u.Property.propertyName : null,
                    isActive = u.a.isActive,
                    TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                    isTaxable = u.a.isTaxable,
                    taxRatePer = u.a.taxRatePer,
                    rentConcession = u.a.rentConcession,
                    messConcession = u.a.messConcession,
                    frequency = u.a.frequency,
                    rentDueDate = u.a.rentDueDate,
                    rentescalationPer = u.a.rentescalationPer,
                    charges = u.a.charges,
                    securityAmt = u.a.securityAmt,
                    bedSpaceID = u.a.bedSpaceID,
                    BedSpaceName = u.BedSpace.bedSpaceName,
                   escalationDate = u.a.escalationDate,


               }).ToListAsync();
            return new PagedResultDto<Models.BedSpaceAssignemnt>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

        }

        public async Task<IEnumerable<Models.BedSpaceAssignemnt>> GetBedSpaceAssignmentbyId(int bedSpaceAssignmentId)
        {

            return await _context.BedSpaceAssignemnt
                .Where(u => u.assignmentID == bedSpaceAssignmentId)
                 .GroupJoin(_context.Properties,
                a => a.propID,
                 p => p.PropID,
                (a, props) => new { a, Property = props.FirstOrDefault() })
            .Join(_context.Units,
                ap => ap.a.unitID,
                u => u.UnitId,
            (ap, unit) => new { ap.a, ap.Property, Unit = unit })
            .Join(_context.Tenants,
                apu => apu.a.tenantID,
                t => t.tenantID,
                (apu, tenant) => new { apu.a, apu.Property, apu.Unit, Tenant = tenant })
            .Join(_context.BedSpaces,
                aput => aput.a.bedSpaceID,
                 b => b.bedID,
                (aput, bedSpace) => new
                {
                    aput.a,
                    aput.Property,
                    aput.Unit,
                    aput.Tenant,
                    BedSpace = bedSpace
                })
                 .Select(u => new Models.BedSpaceAssignemnt
                 {
                     assignmentID = u.a.assignmentID,
                     amount = u.a.amount,
                     agreementStartDate = u.a.agreementStartDate,
                     agreementEndDate = u.a.agreementEndDate,
                     propID = u.a.propID,
                     unitID = u.a.unitID,
                     tenantID = u.a.tenantID,
                     UnitName = u.Unit != null ? u.Unit.UnitName : null,
                     companyID = u.a.companyID,
                     rentAmt = u.a.rentAmt,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     isActive = u.a.isActive,
                     TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                     isTaxable = u.a.isTaxable,
                     taxRatePer = u.a.taxRatePer,
                     rentConcession = u.a.rentConcession,
                     messConcession = u.a.messConcession,
                     frequency = u.a.frequency,
                     rentDueDate = u.a.rentDueDate,
                     rentescalationPer = u.a.rentescalationPer,
                     charges = u.a.charges,
                     securityAmt = u.a.securityAmt,
                     bedSpaceID = u.a.bedSpaceID,
                     BedSpaceName = u.BedSpace.bedSpaceName,
                     escalationDate = u.a.escalationDate,

                 }).ToListAsync();

        }

        public async Task<Models.BedSpaceAssignemnt> CreateBedSpaceAssignemnt(DTOs.BedSpaceAssignment dtoBedSpace)
        {

            var bedSpace = new Models.BedSpaceAssignemnt()
            {
                propID = dtoBedSpace.propID,
                unitID=dtoBedSpace.unitID,
                tenantID=dtoBedSpace.tenantID,
                companyID=dtoBedSpace.companyID,
                rentAmt=dtoBedSpace.rentAmt,
                isTaxable=dtoBedSpace.isTaxable,
                taxRatePer =dtoBedSpace.taxRatePer,
                rentConcession =dtoBedSpace.rentConcession,
                messConcession =dtoBedSpace.messConcession,
                agreementEndDate = dtoBedSpace.agreementEndDate,
                agreementStartDate= dtoBedSpace.agreementStartDate,
                frequency = dtoBedSpace.frequency,
                rentDueDate = dtoBedSpace.rentDueDate,
                rentescalationPer=dtoBedSpace.rentescalationPer,
                escalationDate= dtoBedSpace.escalationDate,
                securityAmt=dtoBedSpace.securityAmt,
                charges=dtoBedSpace.charges,
                amount=dtoBedSpace.amount,
                isActive=dtoBedSpace.isActive,
                bedSpaceID=dtoBedSpace.bedSpaceID,

             };
            await _context.BedSpaceAssignemnt.AddAsync(bedSpace);
            await _context.SaveChangesAsync();
            return bedSpace;

        }

        public async Task<bool> DeleteBedSpaceAssignment(int id)
        {
            var bedspacesettings = await _context.BedSpaceAssignemnt.FirstOrDefaultAsync(u => u.assignmentID == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBedSpaceAssignment(int id, Models.BedSpaceAssignemnt assignemnt)
        {
            var updatebedAssignemnt = await _context.BedSpaceAssignemnt.FirstOrDefaultAsync(u => u.assignmentID == id);
            if (updatebedAssignemnt == null) return false;

            updatebedAssignemnt.amount = assignemnt.amount;
            updatebedAssignemnt.companyID = assignemnt.companyID;
            updatebedAssignemnt.agreementEndDate = assignemnt.agreementEndDate;
            updatebedAssignemnt.agreementStartDate = assignemnt.agreementStartDate;
            updatebedAssignemnt.propID = assignemnt.propID;
            updatebedAssignemnt.rentAmt = assignemnt.rentAmt;
            updatebedAssignemnt.rentConcession = assignemnt.rentConcession;
            updatebedAssignemnt.messConcession = assignemnt.messConcession;
            updatebedAssignemnt.frequency = assignemnt.frequency;
            updatebedAssignemnt.charges = assignemnt.charges;
            updatebedAssignemnt.isTaxable = assignemnt.isTaxable;
            updatebedAssignemnt.escalationDate = assignemnt.escalationDate;
            updatebedAssignemnt.rentescalationPer = assignemnt.rentescalationPer;
            updatebedAssignemnt.taxRatePer = assignemnt.taxRatePer;
            updatebedAssignemnt.tenantID = assignemnt.tenantID;
            updatebedAssignemnt.isActive = assignemnt.isActive;
            updatebedAssignemnt.bedSpaceID = assignemnt.bedSpaceID;
            updatebedAssignemnt.escalationDate = assignemnt.escalationDate;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.BedSpaceAssignemnt>> GetBedSpaceAssignmentAsync(string? propertyName,string unitName,string tenantName,string contactNo, int pageNumber, int pageSize)
        {
            var query = _context.BedSpaceAssignemnt.AsQueryable();

            if (!string.IsNullOrWhiteSpace(unitName))
            {
                query = query.Where(u => u.UnitName.Contains(unitName)); // Adjust property as needed

            }

            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                query = query.Where(u => u.Properties.propertyName.Contains(propertyName));
            }

            if (!string.IsNullOrWhiteSpace(tenantName))
            {
                query = query.Where(u => u.Tenant.tenantName.Contains(tenantName));
            }

            if (!string.IsNullOrWhiteSpace(contactNo))
            {
                query = query.Where(u => u.Tenant.emergencyContactNo.Contains(contactNo));
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.BedSpaceAssignemnt
                {
                    assignmentID = u.assignmentID,
                    agreementStartDate  = u.agreementStartDate,
                    agreementEndDate = u.agreementEndDate,
                    amount = u.amount,
                    charges = u.charges,
                    rentAmt = u.rentAmt,
                    securityAmt = u.securityAmt,
                    unitID=u.unitID,
                    propID=u.propID,
                    tenantID=u.tenantID,
                    companyID=u.companyID,
                    escalationDate=u.escalationDate,
                    frequency=u.frequency,
                    isTaxable=u.isTaxable,
                    messConcession=u.messConcession,
                    rentConcession=u.rentConcession,
                    rentDueDate=u.rentDueDate,
                    rentescalationPer=u.rentescalationPer,
                    taxRatePer=u.taxRatePer,

                })
                .ToListAsync();

            return new PagedResultDto<Models.BedSpaceAssignemnt>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
