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

        public async Task<IEnumerable<Models.Units>> GetUnits()
        {

            return await _context.Units
            .Select(u => new
            {
                Unit = u,
                Property = _context.Properties
            .Where(p => p.PropID == u.PropID)
            .FirstOrDefault(),
                Category = _context.Category
            .Where(c => c.CatID == u.CatID)
            .FirstOrDefault()
            })
             .Select(u => new Models.Units
                  {
                      UnitId = u.Unit.UnitId,
                      UnitName = u.Unit.UnitName,
                      PropID = u.Unit.PropID,
                      CompanyId = u.Unit.CompanyId,
                      UnitType = u.Unit.UnitType,
                      PropName = u.Property != null ? u.Property.propertyName : null,
                      IsActive = u.Unit.IsActive,
                      Area = u.Unit.Area,
                      Remarks = u.Unit.Remarks,
                      FloorNo = u.Unit.FloorNo,
                      DefaultRent = u.Unit.DefaultRent,
                      escalationper = u.Unit.escalationper,
                      CatID=u.Unit.CatID,
                      CategoryName=u.Category != null? u.Category.CategoryName : null,
                  }).ToListAsync();
        }


        public async Task<PagedResultDto<Models.Units>> GetUnitByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.Units.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.CompanyId.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
               .Select(u => new
               {
                   Unit = u,
                   Property = _context.Properties
            .Where(p => p.PropID == u.PropID)
            .FirstOrDefault(),
                   Category = _context.Category
            .Where(c => c.CatID == u.CatID)
            .FirstOrDefault()
               })
                .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
                 .Select(u => new Models.Units
                 {
                     UnitId = u.Unit.UnitId,
                     UnitName = u.Unit.UnitName,
                     PropID = u.Unit.PropID,
                     CompanyId = u.Unit.CompanyId,
                     UnitType = u.Unit.UnitType,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     IsActive = u.Unit.IsActive,
                     Area = u.Unit.Area,
                     Remarks = u.Unit.Remarks,
                     FloorNo = u.Unit.FloorNo,
                     DefaultRent = u.Unit.DefaultRent,
                     escalationper = u.Unit.escalationper,
                     CatID = u.Unit.CatID,
                     CategoryName = u.Category != null ? u.Category.CategoryName : null
                 }).ToListAsync();
            return new PagedResultDto<Models.Units>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

        }

        public async Task<IEnumerable<Models.UnitChargesMapping>> GetUnitChargesByUnitId(int unitId)
        {

            return await _context.UnitChargesMappings
                .Where(u => u.unitMapID == unitId)
                 .Select(u => new Models.UnitChargesMapping
                 {
                     unitMapID = u.unitMapID,
                     unitID = u.unitID,
                     isActive = u.isActive,
                     propID = u.propID,
                     amount = u.amount,
                     frequency = u.frequency,
                     companyID = u.companyID,
                     chargeID = u.chargeID,
                 }).ToListAsync();


        }

        public async Task<DTOs.UnitChargesDTO> GetUnitsByUnitId(int unitId)
        {


            var unit = await _context.Units
                        .Where(u => u.UnitId == unitId)
                        .Select(u => new
                        {
                            Unit = u,
                            Property = _context.Properties
                                         .Where(p => p.PropID == u.PropID)
                                          .FirstOrDefault(),
                            Category = _context.Category
                                        .Where(c => c.CatID == u.CatID)
                                        .FirstOrDefault()
                        })
                        .Select(u => new Models.Units
                         {
                             UnitId = u.Unit.UnitId,
                             UnitName = u.Unit.UnitName,
                             PropID = u.Unit.PropID,
                             CompanyId = u.Unit.CompanyId,
                             UnitType = u.Unit.UnitType,
                             PropName = u.Property != null ? u.Property.propertyName : null,
                             IsActive = u.Unit.IsActive,
                             Area = u.Unit.Area,
                             Remarks = u.Unit.Remarks,
                             FloorNo = u.Unit.FloorNo,
                             DefaultRent = u.Unit.DefaultRent,
                             escalationper = u.Unit.escalationper,
                             CatID = u.Unit.CatID,
                             CategoryName = u.Category != null ? u.Category.CategoryName : null
                         }).FirstOrDefaultAsync();
            var unitmapping = await _context.UnitChargesMappings.Where(u => u.unitID == unitId)
                              .Select(u => new Models.UnitChargesMapping
                              {
                                  chargeID = u.chargeID,
                                  companyID = u.companyID,
                                  amount = u.amount,
                                  frequency = u.frequency,
                                  isActive = u.isActive,
                                  propID = u.propID,
                                  unitID = u.unitID,
                                  ChargeName = u.Charges != null ? u.Charges.chargeName : null,
                                  unitMapID = u.unitMapID,
                                  ChargeType = u.Charges.isVariable ? "Variable" : "Fixed",
                                  

                              }).ToListAsync();



            DTOs.UnitChargesDTO unitChargesDTO = new DTOs.UnitChargesDTO();
            unitChargesDTO.Unit = unit;
            unitChargesDTO.Charges = unitmapping;
            return unitChargesDTO;


        }

        public async Task<DTOs.UnitChargesDTO> GetUnitsByPropertyId(int propertyId)
        {


            var unit = await _context.Units.FirstOrDefaultAsync(u => u.PropID == propertyId);
            var unitmapping = await _context.UnitChargesMappings.Where(u => u.unitID == unit.UnitId)
                              .Select(u => new Models.UnitChargesMapping
                              {
                                  chargeID = u.chargeID,
                                  companyID = u.companyID,
                                  amount = u.amount,
                                  frequency = u.frequency,
                                  isActive = u.isActive,
                                  propID = u.propID,
                                  unitID = u.unitID,
                                  ChargeName = u.Charges != null ? u.Charges.chargeName : null,
                                  unitMapID = u.unitMapID,
                                  ChargeType = u.Charges.isVariable ? "Variable" : "Fixed",

                              }).ToListAsync();



            DTOs.UnitChargesDTO unitChargesDTO = new DTOs.UnitChargesDTO();
            unitChargesDTO.Unit = unit;
            unitChargesDTO.Charges = unitmapping;
            return unitChargesDTO;


        }

        public async Task<Models.Units> CreateUnit(DTOs.CreateUnit dtoUnit)
        {

            var unit = new Models.Units
            {
                UnitName = dtoUnit.UnitName,
                PropID = dtoUnit.PropID,
                UnitType = dtoUnit.UnitType,
                CompanyId = dtoUnit.CompanyId,
                IsActive = dtoUnit.IsActive,
                Area = dtoUnit.Area,
                FloorNo = dtoUnit.FloorNo,
                Remarks = dtoUnit.Remarks,
                escalationper = dtoUnit.escalationper,
                DefaultRent = dtoUnit.DefaultRent,
                CatID=dtoUnit.CatID,

            };
            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;

        }

        public async Task<bool> DeleteUnit(int id)
        {
            var bedspacesettings = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.IsActive = false;
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUnit(DTOs.UnitChargesDTO unitCharges)
        {
            var updateUnits = await _context.Units.FirstOrDefaultAsync(u => u.UnitId == unitCharges.Unit.UnitId);
            if (updateUnits == null) return false;

            updateUnits.UnitType = unitCharges.Unit.UnitType;
            updateUnits.UnitName = unitCharges.Unit.UnitName;
            updateUnits.CompanyId = unitCharges.Unit.CompanyId;
            updateUnits.PropID = unitCharges.Unit.PropID;
            updateUnits.IsActive = unitCharges.Unit.IsActive;
            updateUnits.Area = unitCharges.Unit.Area;
            updateUnits.Remarks = unitCharges.Unit.Remarks;
            updateUnits.FloorNo = unitCharges.Unit.FloorNo;
            updateUnits.DefaultRent = unitCharges.Unit.DefaultRent;
            updateUnits.escalationper = unitCharges.Unit.escalationper;
            updateUnits.CatID = unitCharges.Unit.CatID;

            await _context.SaveChangesAsync();

            foreach (var docDto in unitCharges.Charges)
            {
                if (docDto.unitMapID > 0)
                {
                    // Update existing unit charges mapping
                    var existingUnit = await _context.UnitChargesMappings
                        .Where(d => d.unitMapID == docDto.unitMapID).FirstOrDefaultAsync();

                    if (existingUnit != null)
                    {
                        existingUnit.frequency = docDto.frequency;
                        existingUnit.amount = docDto.amount;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    // Insert new unit charges
                    var unit = new Models.UnitChargesMapping
                    {
                        unitID = docDto.unitID,
                        propID = docDto.propID,
                        amount = docDto.amount,
                        chargeID = docDto.chargeID,
                        frequency = docDto.frequency,
                        isActive = docDto.isActive,
                        companyID = docDto.companyID,
                    };

                    _context.UnitChargesMappings.Add(unit);
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<PagedResultDto<Models.Units>> GetUnitsAsync(string? unitName, string? propetyName, int pageNumber, int pageSize)
        {
            var query = _context.Units
                        .Include(u => u.Properties)
                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(unitName))
            {
                query = query.Where(u => u.UnitName.Contains(unitName)); // Adjust property as needed

            }

            if (!string.IsNullOrWhiteSpace(propetyName))
            {
                query = query.Where(u => u.Properties.propertyName.Contains(propetyName));
            }


            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.Units
                {
                    PropID = u.PropID,
                    UnitName = u.UnitName,
                    UnitId = u.UnitId,
                    UnitType = u.UnitType,
                    IsActive = u.IsActive,
                    PropName = u.Properties != null ? u.Properties.propertyName : null

                })
                .ToListAsync();

            return new PagedResultDto<Models.Units>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<Models.UnitChargesMapping>> GetUnitChargesMapping()
        {

            return await _context.UnitChargesMappings.Where(u => u.isActive == true)
                  .Select(u => new Models.UnitChargesMapping
                  {
                      unitID = u.unitID,
                      propID = u.propID,
                      amount = u.amount,
                      unitMapID = u.unitMapID,
                      companyID = u.companyID,
                      chargeID = u.chargeID,
                      frequency = u.frequency,
                      isActive = u.isActive,

                  }).ToListAsync();
        }

        public async Task<Models.UnitChargesMapping> CreateUnitChargesMapping(DTOs.CreateUnitChargesMapping dtoUnit)
        {

            var unit = new Models.UnitChargesMapping
            {
                unitID = dtoUnit.unitID,
                propID = dtoUnit.propID,
                amount = dtoUnit.amount,
                chargeID = dtoUnit.chargeID,
                frequency = dtoUnit.frequency,
                isActive = dtoUnit.isActive,
                companyID = dtoUnit.companyID,
            };
            await _context.UnitChargesMappings.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;

        }

        public async Task<bool> UpdateUnitChargesMapping(int id, Models.UnitChargesMapping units)
        {
            var updateUnits = await _context.UnitChargesMappings.FirstOrDefaultAsync(u => u.unitMapID == id);
            if (updateUnits == null) return false;

            updateUnits.frequency = units.frequency;
            updateUnits.amount = units.amount;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Units> CreateUnitCharges(UnitCharges dto)
        {
            var unit = new Models.Units
            {
                UnitName = dto.Unit.UnitName,
                PropID = dto.Unit.PropID,
                UnitType = dto.Unit.UnitType,
                CompanyId = dto.Unit.CompanyId,
                IsActive = dto.Unit.IsActive,
                Area = dto.Unit.Area,
                FloorNo = dto.Unit.FloorNo,
                Remarks = dto.Unit.Remarks,
                escalationper = dto.Unit.escalationper,
                DefaultRent = dto.Unit.DefaultRent,
                CatID = dto.Unit.CatID,

            };

            _context.Units.Add(unit);
            await _context.SaveChangesAsync(); // Get TenantId

            foreach (var charges in dto.Charges)
            {

                var unitCharges = new Models.UnitChargesMapping
                {
                    unitID = unit.UnitId,
                    propID = charges.propID,
                    amount = charges.amount,
                    chargeID = charges.chargeID,
                    frequency = charges.frequency,
                    isActive = charges.isActive,
                    companyID = charges.companyID,
                };

                _context.UnitChargesMappings.Add(unitCharges);
                await _context.SaveChangesAsync();

            }

            return unit;
        }
    }
}
