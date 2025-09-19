using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Units
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Models.Units>> GetUnits();
        Task<PagedResultDto<Models.Units>> GetUnitByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Units> CreateUnit(DTOs.CreateUnit unit);

        Task<bool> DeleteUnit(int id);

        Task<DTOs.UnitChargesDTO> GetUnitsByUnitId(int unitId);

        Task<bool> UpdateUnit(DTOs.UnitChargesDTO unitCharges);

        Task<PagedResultDto<Models.Units>> GetUnitsAsync(string? unitName,string? propetyName, int pageNumber, int pageSize);

        Task<IEnumerable<Models.UnitChargesMapping>> GetUnitChargesMapping();

        Task<Models.UnitChargesMapping> CreateUnitChargesMapping(DTOs.CreateUnitChargesMapping unit);

        Task<bool> UpdateUnitChargesMapping(int id, Models.UnitChargesMapping units);

        Task<IEnumerable<Models.UnitChargesMapping>> GetUnitChargesByUnitId(int unitId);

        Task<Models.Units> CreateUnitCharges(DTOs.UnitCharges unit);

        Task<DTOs.UnitChargesDTO> GetUnitsByPropertyId(int propertyId);


    }
}
