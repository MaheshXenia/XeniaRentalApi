using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Company
{
    public interface ICompanyRepsitory
    {
        Task<IEnumerable<Models.XRS_Company>> GetCompanies();
        Task<PagedResultDto<Models.XRS_Company>> GetCompanybyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.XRS_Company> CreateCompany(XeniaRentalApi.Models.XRS_Company company);

        Task<bool> DeleteCompany(int id);

        Task<IEnumerable<Models.XRS_Company>> GetCompanybyId(int companyId);//UpdateCompany


        Task<bool> UpdateCompany(int id, Models.XRS_Company charges);

        Task<PagedResultDto<Models.XRS_Company>> GetCompanyAsync(string? search, int pageNumber, int pageSize);
    }
}
