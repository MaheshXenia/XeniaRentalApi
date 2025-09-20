using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Units
{
    public interface IUnitRepository
    {
        Task<IEnumerable<XRS_Units>> GetUnits(int companyId, int? propertyId = null);

        Task<PagedResultDto<XRS_Units>> GetUnitByCompanyId(int companyId,string? search = null,int pageNumber = 1,int pageSize = 10);

        Task<XRS_Units> GetUnitById(int unitId);

        Task<XRS_Units> CreateUnit(XRS_Units model);

        Task<XRS_Units> UpdateUnit(XRS_Units model);

        Task<bool> DeleteUnit(int id);


    }
}
