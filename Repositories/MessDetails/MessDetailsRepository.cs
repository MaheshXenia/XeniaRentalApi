using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.MessDetails
{
    public class MessDetailsRepository: IMessDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public MessDetailsRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.MessDetails>> GetMessDetails()
        {

            return await _context.MessDetails
                 .GroupJoin(_context.Properties,
        md => md.propID,
        p => p.PropID,
        (md, props) => new { md, props })
    .SelectMany(
        x => x.props.DefaultIfEmpty(),
        (x, prop) => new { x.md, Property = prop })
    .GroupJoin(_context.MessTypes,
        x => x.md.messID,
        mt => mt.messID, // Assuming MessID is the PK in MessTypes
        (x, messTypes) => new { x.md, x.Property, messTypes })
    .SelectMany(
        x => x.messTypes.DefaultIfEmpty(),
        (x, messType) => new { x.md, x.Property, MessType = messType })
    .GroupJoin(_context.Tenants,
        x => x.md.tenantID,
        t => t.tenantID,
        (x, tenants) => new { x.md, x.Property, x.MessType, tenants })
    .SelectMany(
        x => x.tenants.DefaultIfEmpty(),
        (x, tenant) => new
        {
            x.md,
            x.Property,
            x.MessType,
            Tenant = tenant
        })
         .Select(x => new Models.MessDetails
         {
             messdtsID = x.md.messdtsID,
             messID = x.md.messID,
             compantID = x.md.compantID,
             isConsumed = x.md.isConsumed,
             propID = x.md.propID,
             messDate = x.md.messDate,
             tenantID = x.md.tenantID,
             userID = x.md.userID,

             // Optional: enrich with navigation data
             PropName = x.Property != null ? x.Property.propertyName : null,
             MessTypeName = x.MessType != null ? x.MessType.MessName : null,
             TenantName = x.Tenant != null ? x.Tenant.tenantName : null,
         })
        .ToListAsync();
        }


        public async Task<PagedResultDto<Models.MessDetails>> GetMessDetailsByCompanyId(int companyId, int pageNumber, int pageSize)
        {
            var query = _context.MessDetails.AsQueryable();

if (!string.IsNullOrWhiteSpace(companyId.ToString()))
{
    query = query.Where(u => u.compantID == companyId);
}

var totalRecords = await query.CountAsync();

var items = await query
    .GroupJoin(_context.Properties,
        md => md.propID,
        p => p.PropID,
        (md, props) => new { md, props })
    .SelectMany(
        x => x.props.DefaultIfEmpty(),
        (x, prop) => new { x.md, Property = prop })
    .GroupJoin(_context.MessTypes,
        x => x.md.messID,
        mt => mt.messID, // Assuming MessID is the PK in MessTypes
        (x, messTypes) => new { x.md, x.Property, messTypes })
    .SelectMany(
        x => x.messTypes.DefaultIfEmpty(),
        (x, messType) => new { x.md, x.Property, MessType = messType })
    .GroupJoin(_context.Tenants,
        x => x.md.tenantID,
        t => t.tenantID,
        (x, tenants) => new { x.md, x.Property, x.MessType, tenants })
    .SelectMany(
        x => x.tenants.DefaultIfEmpty(),
        (x, tenant) => new
        {
            x.md,
            x.Property,
            x.MessType,
            Tenant = tenant
        })
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .Select(x => new Models.MessDetails
    {
        messdtsID = x.md.messdtsID,
        messID = x.md.messID,
        compantID = x.md.compantID,
        isConsumed = x.md.isConsumed,
        propID = x.md.propID,
        messDate = x.md.messDate,
        tenantID = x.md.tenantID,
        userID = x.md.userID,

        // Optional: enrich with navigation data
        PropName = x.Property != null ? x.Property.propertyName : null,
        MessTypeName = x.MessType != null ? x.MessType.MessName : null,
        TenantName = x.Tenant != null ? x.Tenant.tenantName : null,
    })
    .ToListAsync();
            return new PagedResultDto<Models.MessDetails>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<Models.MessDetails>> GetMessDetailsbyId(int messdetailId)
        {

            return await _context.MessDetails
                .Where(u => u.messdtsID == messdetailId)
                 .GroupJoin(_context.Properties,
        md => md.propID,
        p => p.PropID,
        (md, props) => new { md, props })
    .SelectMany(
        x => x.props.DefaultIfEmpty(),
        (x, prop) => new { x.md, Property = prop })
    .GroupJoin(_context.MessTypes,
        x => x.md.messID,
        mt => mt.messID, // Assuming MessID is the PK in MessTypes
        (x, messTypes) => new { x.md, x.Property, messTypes })
    .SelectMany(
        x => x.messTypes.DefaultIfEmpty(),
        (x, messType) => new { x.md, x.Property, MessType = messType })
    .GroupJoin(_context.Tenants,
        x => x.md.tenantID,
        t => t.tenantID,
        (x, tenants) => new { x.md, x.Property, x.MessType, tenants })
    .SelectMany(
        x => x.tenants.DefaultIfEmpty(),
        (x, tenant) => new
        {
            x.md,
            x.Property,
            x.MessType,
            Tenant = tenant
        })
         .Select(x => new Models.MessDetails
         {
             messdtsID = x.md.messdtsID,
             messID = x.md.messID,
             compantID = x.md.compantID,
             isConsumed = x.md.isConsumed,
             propID = x.md.propID,
             messDate = x.md.messDate,
             tenantID = x.md.tenantID,
             userID = x.md.userID,

             // Optional: enrich with navigation data
             PropName = x.Property != null ? x.Property.propertyName : null,
             MessTypeName = x.MessType != null ? x.MessType.MessName : null,
             TenantName = x.Tenant != null ? x.Tenant.tenantName : null,
         })
        .ToListAsync();

        }

        public async Task<Models.MessDetails> CreateMessDetails(Models.MessDetails details)
        {

            await _context.MessDetails.AddAsync(details);
            await _context.SaveChangesAsync();
            return details;

        }

        public async Task<bool> UpdateMessDetails(int id, Models.MessDetails details)
        {
            var updatedMessDetails = await _context.MessDetails.FirstOrDefaultAsync(u => u.messdtsID == id);
            if (updatedMessDetails == null) return false;

            updatedMessDetails.messDate = details.messDate;
            updatedMessDetails.messID = details.messID;
            updatedMessDetails.propID = details.propID;
            updatedMessDetails.isConsumed = details.isConsumed;
            updatedMessDetails.compantID = details.compantID;
            updatedMessDetails.userID = details.userID;
            updatedMessDetails.tenantID = details.tenantID;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
