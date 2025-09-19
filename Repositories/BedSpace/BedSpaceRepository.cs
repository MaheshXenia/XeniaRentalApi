using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.BedSpace
{
    public class BedSpaceRepository: IBedSpaceRepository
    {
        private readonly ApplicationDbContext _context;
        public BedSpaceRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.BedSpace>> GetBedSpaces()
        {

            return await _context.BedSpaces
                .GroupJoin(
         _context.Properties,
         doc => doc.propID,
         prop => prop.PropID,
         (doc, props) => new { doc, prop = props.FirstOrDefault() }
     )
     .Join(
         _context.Units,
         combined => combined.doc.unitID,
         unit => unit.UnitId,
         (combined, unit) => new
         {
             Document = combined.doc,
             Property = combined.prop,
             Unit = unit
         }
     )
                 .Select(u => new Models.BedSpace
                 {
                     bedID = u.Document.bedID,
                     bedSpaceName = u.Document.bedSpaceName,
                     propID = u.Document.propID,
                     unitID = u.Document.unitID,
                     planID = u.Document.planID,
                     UnitName = u.Unit != null ? u.Unit.UnitName : null,
                     companyID = u.Document.companyID,
                     rentAmt = u.Document.rentAmt,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     isActive = u.Document.isActive,

                 }).ToListAsync();
               

        }

        public async Task<PagedResultDto<Models.BedSpace>> GetBedSpacebyCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.BedSpaces.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
     .GroupJoin(
         _context.Properties,
         doc => doc.propID,
         prop => prop.PropID,
         (doc, props) => new { doc, prop = props.FirstOrDefault() }
     )
     .Join(
         _context.Units,
         combined => combined.doc.unitID,
         unit => unit.UnitId,
         (combined, unit) => new
         {
             Document = combined.doc,
             Property = combined.prop,
             Unit = unit
         }
     )
       .OrderBy(x => x.Document.bedSpaceName)
     .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
     .Select(u => new Models.BedSpace
     {
         bedID = u.Document.bedID,
         bedSpaceName = u.Document.bedSpaceName,
         companyID = u.Document.companyID,
         planID = u.Document.planID,
         propID = u.Document.propID,
         rentAmt = u.Document.rentAmt,
         unitID = u.Document.unitID,
         UnitName = u.Unit != null ? u.Unit.UnitName : null,
         PropName = u.Property != null ? u.Property.propertyName : null,
         isActive = u.Document.isActive,
     })
     .ToListAsync();
            return new PagedResultDto<Models.BedSpace>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

        }

        public async Task<IEnumerable<Models.BedSpace>> GetBedSpacebyId(int bedSpaceId)
        {

            return await _context.BedSpaces
                .Where(u => u.bedID == bedSpaceId)
                .GroupJoin(
         _context.Properties,
         doc => doc.propID,
         prop => prop.PropID,
         (doc, props) => new { doc, prop = props.FirstOrDefault() }
     )
     .Join(
         _context.Units,
         combined => combined.doc.unitID,
         unit => unit.UnitId,
         (combined, unit) => new
         {
             Document = combined.doc,
             Property = combined.prop,
             Unit = unit
         }
     )
                 .Select(u => new Models.BedSpace
                 {
                     bedID = u.Document.bedID,
                     bedSpaceName = u.Document.bedSpaceName,
                     propID = u.Document.propID,
                     unitID = u.Document.unitID,
                     planID = u.Document.planID,
                     UnitName = u.Unit != null ? u.Unit.UnitName : null,
                     companyID = u.Document.companyID,
                     rentAmt = u.Document.rentAmt,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     isActive = u.Document.isActive,

                 }).ToListAsync();

        }

        public async Task<IEnumerable<Models.BedSpace>> GetBedSpacebyUnitId(int unitId)
        {

            return await _context.BedSpaces
                .Where(u => u.unitID == unitId)
                .GroupJoin(
         _context.Properties,
         doc => doc.propID,
         prop => prop.PropID,
         (doc, props) => new { doc, prop = props.FirstOrDefault() }
     )
     .Join(
         _context.Units,
         combined => combined.doc.unitID,
         unit => unit.UnitId,
         (combined, unit) => new
         {
             Document = combined.doc,
             Property = combined.prop,
             Unit = unit
         }
     )
                 .Select(u => new Models.BedSpace
                 {
                     bedID = u.Document.bedID,
                     bedSpaceName = u.Document.bedSpaceName,
                     propID = u.Document.propID,
                     unitID = u.Document.unitID,
                     planID = u.Document.planID,
                     UnitName = u.Unit != null ? u.Unit.UnitName : null,
                     companyID = u.Document.companyID,
                     rentAmt = u.Document.rentAmt,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     isActive = u.Document.isActive,

                 }).ToListAsync();

        }

        public async Task<Models.BedSpace> CreateBedSpaces(DTOs.CreateBedSpace dtoBedSpace)
        {

            var bedSpace = new Models.BedSpace
            {
               companyID = dtoBedSpace.companyID,
               propID = dtoBedSpace.propID,
               unitID = dtoBedSpace.unitID,
               planID = dtoBedSpace.planID,
               bedSpaceName = dtoBedSpace.bedSpaceName,
               rentAmt = dtoBedSpace.rentAmt,
               isActive=dtoBedSpace.isActive

            };
            await _context.BedSpaces.AddAsync(bedSpace);
            await _context.SaveChangesAsync();
            return bedSpace;

        }

        public async Task<bool> DeleteBedSpace(int id)
        {
            var bedspacesettings = await _context.BedSpaces.FirstOrDefaultAsync(u => u.bedID == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBedSpace(int id, Models.BedSpace bedSpace)
        {
            var updatebedSpace = await _context.BedSpaces.FirstOrDefaultAsync(u => u.bedID == id);
            if (updatebedSpace == null) return false;

            updatebedSpace.bedSpaceName = bedSpace.bedSpaceName ?? bedSpace.bedSpaceName;
            updatebedSpace.companyID = bedSpace.companyID;
            updatebedSpace.planID = bedSpace.planID;
            updatebedSpace.propID = bedSpace.propID;
            updatebedSpace.unitID = bedSpace.unitID;
            updatebedSpace.rentAmt = bedSpace.rentAmt;
            updatebedSpace.isActive = bedSpace.isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.BedSpace>> GetBedSpaceAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.BedSpaces.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.bedSpaceName.Contains(search)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                 .GroupJoin(
                 _context.Properties,
                 doc => doc.propID,
                prop => prop.PropID,
                (doc, props) => new { doc, prop = props.FirstOrDefault() }
             )
        .Join(
         _context.Units,
         combined => combined.doc.unitID,
         unit => unit.UnitId,
         (combined, unit) => new
         {
             Document = combined.doc,
             Property = combined.prop,
             Unit = unit
         }
        )
                .OrderBy(u => u.Document.bedSpaceName) // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.BedSpace
                {
                    bedID = u.Document.bedID,
                    bedSpaceName = u.Document.bedSpaceName,
                    companyID = u.Document.companyID,
                    planID = u.Document.planID,
                    propID = u.Document.propID,
                    rentAmt = u.Document.rentAmt,
                    unitID = u.Document.unitID,
                    UnitName = u.Unit!= null ? u.Unit.UnitName.ToString() : null,
                    PropName = u.Property != null ? u.Property.propertyName : null,
                    isActive = u.Document.isActive,
                })
                .ToListAsync();

            return new PagedResultDto<Models.BedSpace>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
