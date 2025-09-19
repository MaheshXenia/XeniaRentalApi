using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Company
{
    public interface ICompanyRepsitory
    {
        Task<IEnumerable<Models.Company>> GetCompanies();
        Task<PagedResultDto<Models.Company>> GetCompanybyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Company> CreateCompany(XeniaRentalApi.Models.Company company);

        Task<bool> DeleteCompany(int id);

        Task<IEnumerable<Models.Company>> GetCompanybyId(int companyId);//UpdateCompany


        Task<bool> UpdateCompany(int id, Models.Company charges);

        Task<PagedResultDto<Models.Company>> GetCompanyAsync(string? search, int pageNumber, int pageSize);
    }
}
