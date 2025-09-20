using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Charges
{
    public class ChargesRepository:IChargesRepository
    {
        private readonly ApplicationDbContext _context;
        public ChargesRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<ChargesDto>> GetCharges(int companyId, int? propertyId = null)
        {
            var query = _context.Charges
                .AsNoTracking()
                .Where(c => c.companyID == companyId);

            if (propertyId.HasValue)
            {
                query = query.Where(c => c.PropID == propertyId.Value); 
            }

            return await query
                .Select(c => new ChargesDto
                {
                    chargeID = c.chargeID,
                    chargeName = c.chargeName,
                    PropID = c.PropID,
                    companyID = c.companyID,
                    chargeAmt = c.chargeAmt,
                    isVariable = c.isVariable,
                    isActive = c.isActive,
                    PropName = c.Property != null ? c.Property.propertyName : null
                })
                .ToListAsync();
        }


        public async Task<PagedResultDto<ChargesDto>> GetChargesByCompanyId(int companyId, int pageNumber, int pageSize)
        {
            var query = _context.Charges
                .Where(c => c.companyID == companyId)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

 
            var items = await query
                .GroupJoin(
                    _context.Properties,
                    charges => charges.PropID,
                    prop => prop.PropID,
                    (charges, props) => new { charges, prop = props.FirstOrDefault() }
                )
                .OrderBy(x => x.charges.chargeName) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new ChargesDto
                {
                    chargeID = u.charges.chargeID,
                    chargeName = u.charges.chargeName,
                    PropID = u.charges.PropID,
                    companyID = u.charges.companyID,
                    chargeAmt = u.charges.chargeAmt,
                    isActive = u.charges.isActive,
                    isVariable = u.charges.isVariable,
                    PropName = u.prop != null ? u.prop.propertyName : null
                })
                .ToListAsync();

            return new PagedResultDto<ChargesDto>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<ChargesDto?> GetChargesById(int chargeId)
        {
            return await _context.Charges
                .Where(c => c.chargeID == chargeId)
                .GroupJoin(
                    _context.Properties,
                    charges => charges.PropID,
                    prop => prop.PropID,
                    (charges, props) => new { charges, prop = props.FirstOrDefault() }
                )
                .Select(u => new ChargesDto
                {
                    chargeID = u.charges.chargeID,
                    chargeName = u.charges.chargeName,
                    PropID = u.charges.PropID,
                    companyID = u.charges.companyID,
                    chargeAmt = u.charges.chargeAmt,
                    isActive = u.charges.isActive,
                    isVariable = u.charges.isVariable,
                    PropName = u.prop != null ? u.prop.propertyName : null
                })
                .FirstOrDefaultAsync();
        }


        public async Task<Models.XRS_Charges> CreateCharges(DTOs.ChargesDto createCharges)
        {

            var charge = new Models.XRS_Charges
            {
                chargeName= createCharges.chargeName,
                chargeAmt = createCharges.chargeAmt,
                PropID = createCharges.PropID,
                companyID = createCharges.companyID,
                isActive = createCharges.isActive,
                isVariable = createCharges.isVariable

            };
            await _context.Charges.AddAsync(charge);
            await _context.SaveChangesAsync();
            return charge;

        }

        public async Task<bool> DeleteCharges(int id)
        {
            var charger = await _context.Charges.FirstOrDefaultAsync(u => u.chargeID == id);
            if (charger == null) return false;
            charger.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCharges(int id, Models.XRS_Charges charges)
        {
            var updatedCharges = await _context.Charges.FirstOrDefaultAsync(u => u.chargeID == id);
            if (updatedCharges == null) return false;

            updatedCharges.chargeName = charges.chargeName ?? charges.chargeName;
            updatedCharges.companyID = charges.companyID;
            updatedCharges.PropID = charges.PropID;
            updatedCharges.isVariable = charges.isVariable;
            updatedCharges.chargeAmt = charges.chargeAmt;
            updatedCharges.isActive = charges.isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.XRS_Charges>> GetChargesAsync(string? chargeName,string? propertyName, int pageNumber, int pageSize)
        {
            var query = _context.Charges
                 .Include(u => u.Property)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(chargeName))
            {
                query = query.Where(u => u.chargeName.Contains(chargeName)); // Adjust property as needed

            }

            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                query = query.Where(u => u.Property.propertyName.Contains(propertyName));
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.XRS_Charges
                {
                    chargeID=u.chargeID,
                    chargeName=u.chargeName,
                    companyID=u.companyID,
                    isVariable=u.isVariable,
                    chargeAmt=u.chargeAmt,
                    isActive=u.isActive,
                    PropID=u.PropID,
                    //PropName = u.Property != null ? u.Property.propertyName : null
                })
                .ToListAsync();

            return new PagedResultDto<Models.XRS_Charges>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
