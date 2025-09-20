﻿using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Tenant
{
    public interface ITenantRepository
    {
        Task<IEnumerable<XRS_Tenant>> GetTenants(int companyId);
        Task<PagedResultDto<TenantGetDto>> GetTenantsByCompanyId(int companyId, bool? status = null, string? search = null, int pageNumber = 1, int pageSize = 10);
        Task<TenantGetDto> GetTenantWithDocumentsById(int tenantId);
        Task<XRS_Tenant> CreateTenant(TenantCreateDto tenantDto);
        Task<bool> UpdateTenant(int tenantId, TenantCreateDto tenantDto);
        Task<Dictionary<string, string>> UploadFilesAsync(List<IFormFile> files);
        Task<bool> DeleteTenant(int id);



    }
}
