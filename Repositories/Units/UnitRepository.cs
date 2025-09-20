using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Units
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<XRS_Units>> GetUnits(int companyId, int? propertyId = null)
        {
            var query = _context.Units.AsQueryable();
            query = query.Where(p => p.CompanyId == companyId);

            if (propertyId.HasValue)
            {
                query = query.Where(p => p.PropID == propertyId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<PagedResultDto<XRS_Units>> GetUnitByCompanyId(int companyId, string? search = null, int pageNumber = 1, int pageSize = 10)
        {

            var query = _context.Units
                .Include(u => u.Property)
                .Include(u => u.Category)
                .Include(u => u.UnitCharges)
                    .ThenInclude(uc => uc.Charges)
                .Where(u => u.CompanyId == companyId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(u =>
                    u.UnitName.ToLower().Contains(lowerSearch) ||
                    (u.Property != null && u.Property.propertyName.ToLower().Contains(lowerSearch)));
            }

            var totalRecords = await query.CountAsync();

            var units = await query
                .OrderBy(u => u.UnitName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var items = units.Select(u => new XRS_Units
            {
                UnitId = u.UnitId,
                UnitName = u.UnitName,
                PropID = u.PropID,
                CompanyId = u.CompanyId,
                UnitType = u.UnitType,
                PropName = u.Property != null ? u.Property.propertyName : null,
                IsActive = u.IsActive,
                Area = u.Area,
                Remarks = u.Remarks,
                FloorNo = u.FloorNo,
                DefaultRent = u.DefaultRent,
                escalationper = u.escalationper,
                CatID = u.CatID,
                CategoryName = u.Category != null ? u.Category.CategoryName : null,
                UnitCharges = u.UnitCharges?.Select(uc => new XRS_UnitChargesMapping
                {
                    unitMapID = uc.unitMapID,
                    unitID = uc.unitID,
                    propID = uc.propID,
                    companyID = uc.companyID,
                    chargeID = uc.chargeID,
                    amount = uc.amount,
                    frequency = uc.frequency,
                    isActive = uc.isActive,
                    ChargeName = uc.Charges?.chargeName,
                    ChargeType = uc.Charges != null && uc.Charges.isVariable ? "Variable" : "Fixed"
                }).ToList()
            }).ToList();

            return new PagedResultDto<XRS_Units>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<XRS_Units> GetUnitById(int unitId)
        {
            var unit = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.Category)
                .Include(u => u.UnitCharges)
                    .ThenInclude(uc => uc.Charges)
                .Where(u => u.UnitId == unitId)
                .FirstOrDefaultAsync();

            if (unit == null)
                return null;

            var result = new XRS_Units
            {
                UnitId = unit.UnitId,
                UnitName = unit.UnitName,
                PropID = unit.PropID,
                CompanyId = unit.CompanyId,
                UnitType = unit.UnitType,
                PropName = unit.Property?.propertyName,
                CatID = unit.CatID,
                CategoryName = unit.Category?.CategoryName,
                IsActive = unit.IsActive,
                Area = unit.Area,
                Remarks = unit.Remarks,
                FloorNo = unit.FloorNo,
                DefaultRent = unit.DefaultRent,
                escalationper = unit.escalationper,
                UnitCharges = unit.UnitCharges?.Select(uc => new Models.XRS_UnitChargesMapping
                {
                    unitMapID = uc.unitMapID,
                    unitID = uc.unitID,
                    propID = uc.propID,
                    companyID = uc.companyID,
                    chargeID = uc.chargeID,
                    amount = uc.amount,
                    frequency = uc.frequency,
                    isActive = uc.isActive,
                    ChargeName = uc.Charges?.chargeName,
                    ChargeType = uc.Charges != null && uc.Charges.isVariable ? "Variable" : "Fixed"
                }).ToList()
            };

            return result;
        }

        public async Task<XRS_Units> CreateUnit(XRS_Units model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var unit = new XRS_Units
            {
                UnitName = model.UnitName,
                PropID = model.PropID,
                CompanyId = model.CompanyId,
                UnitType = model.UnitType,
                CatID = model.CatID,
                IsActive = model.IsActive,
                Area = model.Area,
                Remarks = model.Remarks,
                FloorNo = model.FloorNo,
                DefaultRent = model.DefaultRent,
                escalationper = model.escalationper
            };


            if (model.UnitCharges != null && model.UnitCharges.Any())
            {
                unit.UnitCharges = model.UnitCharges.Select(uc => new XRS_UnitChargesMapping
                {
                    unitID = uc.unitID,
                    propID = uc.propID,
                    companyID = uc.companyID,
                    chargeID = uc.chargeID,
                    amount = uc.amount,
                    frequency = uc.frequency,
                    isActive = uc.isActive
                }).ToList();
            }

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return await GetUnitById(unit.UnitId);
        }

        public async Task<XRS_Units> UpdateUnit(XRS_Units model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var unit = await _context.Units
                .Include(u => u.UnitCharges)
                .FirstOrDefaultAsync(u => u.UnitId == model.UnitId);

            if (unit == null)
                return null;

            unit.UnitName = model.UnitName;
            unit.PropID = model.PropID;
            unit.CompanyId = model.CompanyId;
            unit.UnitType = model.UnitType;
            unit.CatID = model.CatID;
            unit.IsActive = model.IsActive;
            unit.Area = model.Area;
            unit.Remarks = model.Remarks;
            unit.FloorNo = model.FloorNo;
            unit.DefaultRent = model.DefaultRent;
            unit.escalationper = model.escalationper;


            if (model.UnitCharges != null)
            {
                var chargesToRemove = unit.UnitCharges
                    .Where(uc => !model.UnitCharges.Any(m => m.unitMapID == uc.unitMapID))
                    .ToList();
                _context.UnitChargesMappings.RemoveRange(chargesToRemove);


                foreach (var ucModel in model.UnitCharges)
                {
                    var existingCharge = unit.UnitCharges
                        .FirstOrDefault(uc => uc.unitMapID == ucModel.unitMapID);

                    if (existingCharge != null)
                    {
                        existingCharge.chargeID = ucModel.chargeID;
                        existingCharge.amount = ucModel.amount;
                        existingCharge.frequency = ucModel.frequency;
                        existingCharge.isActive = ucModel.isActive;
                    }
                    else
                    {
                        unit.UnitCharges.Add(new XRS_UnitChargesMapping
                        {
                            unitID = unit.UnitId,
                            propID = ucModel.propID,
                            companyID = ucModel.companyID,
                            chargeID = ucModel.chargeID,
                            amount = ucModel.amount,
                            frequency = ucModel.frequency,
                            isActive = ucModel.isActive
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return await GetUnitById(unit.UnitId);
        }

        public async Task<bool> DeleteUnit(int id)
        {
            var bedspacesettings = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.IsActive = false;        
            await _context.SaveChangesAsync();
            return true;
        }

    }
 
}
