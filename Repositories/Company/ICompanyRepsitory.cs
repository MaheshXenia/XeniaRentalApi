using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Company
{
    public interface ICompanyRepsitory
    {
        Task<IEnumerable<XRS_Company>> GetCompanies(int pageNumber, int pageSize);

        Task<XRS_Company> CreateCompany(XRS_Company company);

        Task<bool> DeleteCompany(int id);

        Task<IEnumerable<XRS_Company>> GetCompanybyId(int companyId);


        Task<bool> UpdateCompany(int id, XRS_Company charges);

    }
}
