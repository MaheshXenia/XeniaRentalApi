using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;


namespace XeniaRentalApi.Repositories.Company
{
    public class CompanyRepository:ICompanyRepsitory
    {

        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Models.Company>> GetCompanies()
        {

            return await _context.Company.ToListAsync();

        }


        public async Task<PagedResultDto<Models.Company>> GetCompanybyCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.Company.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();
            var items = await query
               .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
            .Select(u => new Models.Company
            {
                companyID = companyId,
                companyName = u.companyName,
                address = u.address,
                email = u.email,
                IsActive = u.IsActive,
                logo = u.logo,
                phoneNumber = u.phoneNumber,
                pin = u.pin,

            })
                .ToListAsync();
            return new PagedResultDto<Models.Company>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

        }

        public async Task<IEnumerable<Models.Company>> GetCompanybyId(int companyId)
        {

            return await _context.Company
                .Where(u => u.companyID == companyId)
                 .ToListAsync();

        }

        public async Task<Models.Company> CreateCompany(Models.Company company)
        {

            await _context.Company.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;

        }

        public async Task<bool> DeleteCompany(int id)
        {
            var bedspacesettings = await _context.Company.FirstOrDefaultAsync(u => u.companyID == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.IsActive = false;
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCompany(int id, Models.Company company)
        {
            var updatedCompany = await _context.Company.FirstOrDefaultAsync(u => u.companyID == id);
            if (updatedCompany == null) return false;

            updatedCompany.companyName = company.companyName ?? company.companyName;
            updatedCompany.companyID = company.companyID;
            updatedCompany.phoneNumber = company.phoneNumber;
            updatedCompany.address = company.address;
            updatedCompany.pin = company.pin;
            updatedCompany.email = company.email;
            updatedCompany.logo = company.logo;
            updatedCompany.IsActive = company.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<PagedResultDto<Models.Company>> GetCompanyAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Company.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.companyName.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.Company
                {
                   companyID = u.companyID,
                   companyName = u.companyName,
                   phoneNumber = u.phoneNumber,
                   address = u.address,
                   pin = u.pin,
                   email = u.email,
                   logo = u.logo,
                   IsActive = u.IsActive,


                })
                .ToListAsync();

            return new PagedResultDto<Models.Company>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
