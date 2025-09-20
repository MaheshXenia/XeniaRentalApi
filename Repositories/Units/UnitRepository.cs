using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Units;

namespace XeniaRentalApi.Repositories.Unit
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _context;

        public UnitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UnitDto>> GetUnits(int companyId, int? propertyId = null)
        {
            var query = _context.Units
                .AsNoTracking()
                .Include(u => u.Property)
                .Include(u => u.Category)
                .Where(u => u.CompanyId == companyId && u.IsActive);


            if (propertyId.HasValue)
            {
                query = query.Where(u => u.PropID == propertyId.Value);
            }

 
            var units = await query
                .OrderBy(u => u.UnitName)
                .ToListAsync();

       
            var unitsDto = units.Select(u => new UnitDto
            {
                UnitId = u.UnitId,
                UnitName = u.UnitName,
                PropID = u.PropID,
                CompanyId = u.CompanyId,
                UnitType = u.UnitType,
                PropName = u.Property?.propertyName,
                CatID = u.CatID,
                CategoryName = u.Category?.CategoryName,
                IsActive = u.IsActive,
                Area = u.Area,
                Remarks = u.Remarks,
                FloorNo = u.FloorNo,
                DefaultRent = u.DefaultRent,
                escalationper = u.escalationper,
                UnitCharges = null 
            }).ToList();

            return unitsDto;
        }


        public async Task<PagedResultDto<UnitDto>> GetUnitByCompanyId(int companyId, string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Units
                .Include(u => u.Property)
                .Include(u => u.Category)
                .Where(u => u.CompanyId == companyId && u.IsActive)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(u =>
                    u.UnitName.ToLower().Contains(lowerSearch) ||
                    (u.Property != null && u.Property.propertyName.ToLower().Contains(lowerSearch)) ||
                    (u.Category != null && u.Category.CategoryName.ToLower().Contains(lowerSearch))
                );
            }

            var totalRecords = await query.CountAsync();

            var unitsInMemory = await query
                .OrderBy(u => u.UnitName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var unitIds = unitsInMemory.Select(u => u.UnitId).ToList();

            var unitChargesDict = await _context.UnitChargesMappings
                .Where(uc => unitIds.Contains(uc.unitID))
                .Include(uc => uc.Charges)
                .ToListAsync();

            var unitsDto = unitsInMemory.Select(u =>
            {
                var charges = unitChargesDict
                    .Where(uc => uc.unitID == u.UnitId)
                    .Select(uc => new UnitChargeDto
                    {
                        unitMapID = uc.unitMapID,
                        chargeID = uc.chargeID,
                        amount = uc.amount,
                        frequency = uc.frequency,
                        isActive = uc.isActive,
                        ChargeName = uc.Charges?.chargeName,
                        ChargeType = uc.Charges != null && uc.Charges.isVariable ? "Variable" : "Fixed"
                    }).ToList();

                return new UnitDto
                {
                    UnitId = u.UnitId,
                    UnitName = u.UnitName,
                    PropID = u.PropID,
                    CompanyId = u.CompanyId,
                    UnitType = u.UnitType,
                    PropName = u.Property?.propertyName,
                    CatID = u.CatID,
                    CategoryName = u.Category?.CategoryName,
                    IsActive = u.IsActive,
                    Area = u.Area,
                    Remarks = u.Remarks,
                    FloorNo = u.FloorNo,
                    DefaultRent = u.DefaultRent,
                    escalationper = u.escalationper,
                    UnitCharges = charges
                };
            }).ToList();

            return new PagedResultDto<UnitDto>
            {
                Data = unitsDto,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<UnitDto> GetUnitById(int unitId)
        {
            var unit = await _context.Units
                .Include(u => u.Property)
                .Include(u => u.Category)
                .Include(u => u.UnitCharges)
                    .ThenInclude(uc => uc.Charges)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UnitId == unitId);

            if (unit == null)
                return null;

            var charges = unit.UnitCharges?.Select(uc => new UnitChargeDto
            {
                unitMapID = uc.unitMapID,
                chargeID = uc.chargeID,
                amount = uc.amount,
                frequency = uc.frequency,
                isActive = uc.isActive,
                ChargeName = uc.Charges?.chargeName,
                ChargeType = uc.Charges != null && uc.Charges.isVariable ? "Variable" : "Fixed"
            }).ToList();

            return new UnitDto
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
                UnitCharges = charges
            };
        }

        public async Task<UnitDto> CreateUnit(UnitDto model)
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

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            if (model.UnitCharges != null && model.UnitCharges.Any())
            {
                var unitCharges = model.UnitCharges.Select(uc => new XRS_UnitChargesMapping
                {
                    unitID = unit.UnitId,
                    propID = unit.PropID,
                    companyID = unit.CompanyId,
                    chargeID = uc.chargeID,
                    amount = uc.amount,
                    frequency = uc.frequency,
                    isActive = uc.isActive
                }).ToList();

                _context.UnitChargesMappings.AddRange(unitCharges);
                await _context.SaveChangesAsync();
            }

            return await GetUnitById(unit.UnitId);
        }

        public async Task<UnitDto> UpdateUnit(UnitDto model)
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
                    var existing = unit.UnitCharges
                        .FirstOrDefault(uc => uc.unitMapID == ucModel.unitMapID);

                    if (existing != null)
                    {
                        existing.chargeID = ucModel.chargeID;
                        existing.amount = ucModel.amount;
                        existing.frequency = ucModel.frequency;
                        existing.isActive = ucModel.isActive;
                    }
                    else
                    {
                        unit.UnitCharges.Add(new XRS_UnitChargesMapping
                        {
                            unitID = unit.UnitId,
                            propID = unit.PropID,
                            companyID = unit.CompanyId,
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

        public async Task<bool> DeleteUnit(int unitId)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == unitId);
            if (unit == null) return false;

            unit.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
