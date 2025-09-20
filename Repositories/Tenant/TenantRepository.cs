using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Net;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Service.Common;
using static System.Net.WebRequestMethods;

namespace XeniaRentalApi.Repositories.Tenant
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly FtpSettings _ftp;
        public TenantRepository(ApplicationDbContext context, IOptions<FtpSettings> ftpOptions)
        {
            _context = context;
            _ftp = ftpOptions.Value;
        }

        public async Task<IEnumerable<XRS_Tenant>> GetTenants(int companyId)
        {
            return await _context.Tenants
                .AsNoTracking()
                .Where(t => t.companyID == companyId)
                .Include(t => t.Properties) 
                .Include(t => t.Units)     
                .Select(t => new XRS_Tenant
                {
                    tenantID = t.tenantID,
                    tenantName = t.tenantName,
                    propID = t.propID,
                    companyID = t.companyID,
                    unitID = t.unitID,
                    phoneNumber = t.phoneNumber,
                    email = t.email,
                    emergencyContactNo = t.emergencyContactNo,
                    concessionper = t.concessionper,
                    note = t.note,
                    address = t.address,
                    isActive = t.isActive,
                    PropName = t.Properties != null ? t.Properties.propertyName : null,
                    UnitName = t.Units != null ? t.Units.UnitName : null
                })
                .ToListAsync();
        }

        public async Task<PagedResultDto<TenantGetDto>> GetTenantsByCompanyId(int companyId,bool? status = null,string? search = null,int pageNumber = 1, int pageSize = 1)
        {
            var query = _context.Tenants
                .Include(t => t.Properties)
                .Include(t => t.Units)
                .Include(t => t.TenantDocuments)
                    .ThenInclude(td => td.Documents)
                .Where(t => t.companyID == companyId)
                .AsNoTracking();

            // Apply status filter
            if (status.HasValue)
            {
                query = query.Where(t => t.isActive == status.Value);
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(t =>
                    t.tenantName.ToLower().Contains(lowerSearch) ||
                    t.phoneNumber.ToLower().Contains(lowerSearch) ||
                    t.email.ToLower().Contains(lowerSearch)
                );
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(t => t.tenantName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TenantGetDto
                {
                    Tenant = new XRS_Tenant
                    {
                        tenantID = t.tenantID,
                        tenantName = t.tenantName,
                        companyID = t.companyID,
                        propID = t.propID,
                        unitID = t.unitID,
                        email = t.email,
                        phoneNumber = t.phoneNumber,
                        emergencyContactNo = t.emergencyContactNo,
                        address = t.address,
                        note = t.note,
                        concessionper = t.concessionper,
                        isActive = t.isActive,
                        PropName = t.Properties != null ? t.Properties.propertyName : null,
                        UnitName = t.Units != null ? t.Units.UnitName : null
                    },
                    Documents = t.TenantDocuments
                        .Select(td => new TenantDocumentDto
                        {
                            TenantID = td.TenantID,
                            DocTypeId = td.DocTypeId,
                            CompanyID = td.CompanyID,
                            DocumentsNo = td.DocumentsNo,
                            DocumentUrl = td.Documenturl,
                            IsActive = td.isActive,
                            DocumentName = td.Documents.docName,
                            IsAlphaNumeric = td.Documents.isAlphanumeric,
                            IsMandatory = td.Documents.isMandatory,
                            IsExpiry = td.Documents.isExpiry,
                            DocPurpose = td.Documents.docPurpose,
                            ExpiryDate = td.Documents.ExpiryDate
                        })
                        .ToList()
                })
                .ToListAsync();

            return new PagedResultDto<TenantGetDto>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<TenantGetDto> GetTenantWithDocumentsById(int tenantId)
        {
            var tenant = await _context.Tenants
                .Include(t => t.Properties)
                .Include(t => t.Units)
                .Where(t => t.tenantID == tenantId)
                .FirstOrDefaultAsync();

            if (tenant == null)
            {
                return new TenantGetDto
                {
                    Tenant = null,
                    Documents = new List<TenantDocumentDto>()
                };
            }

            var documents = await _context.TenantDocuments
                .Include(td => td.Documents)
                .Where(td => td.TenantID == tenantId)
                .Select(td => new TenantDocumentDto
                {
                    TenantID = td.TenantID,
                    DocTypeId = td.DocTypeId,
                    CompanyID = td.CompanyID,
                    DocumentsNo = td.DocumentsNo,
                    DocumentUrl = td.Documenturl,
                    IsActive = td.isActive,
                    DocumentName = td.Documents.docName,
                    IsAlphaNumeric = td.Documents.isAlphanumeric,
                    IsMandatory = td.Documents.isMandatory,
                    IsExpiry = td.Documents.isExpiry,
                    DocPurpose = td.Documents.docPurpose,
                    ExpiryDate = td.Documents.ExpiryDate
                }).ToListAsync();

            return new TenantGetDto
            {
                Tenant = tenant,
                Documents = documents
            };
        }

        public async Task<XRS_Tenant> CreateTenant(TenantCreateDto tenantDto)
        {
            var tenant = new XRS_Tenant
            {
                tenantName = tenantDto.tenantName,
                unitID = tenantDto.unitID,
                propID = tenantDto.propID,
                companyID = tenantDto.companyID,
                phoneNumber = tenantDto.phoneNumber,
                email = tenantDto.email,
                emergencyContactNo = tenantDto.emergencyContactNo,
                concessionper = tenantDto.concessionper,
                note = tenantDto.note,
                address = tenantDto.address,
                isActive = tenantDto.isActive,
            };

            if (tenantDto.TenantDocuments != null && tenantDto.TenantDocuments.Any())
            {
                tenant.TenantDocuments = tenantDto.TenantDocuments.Select(td => new XRS_TenantDocuments
                {
                    DocTypeId = td.docTypeId,
                    DocumentsNo = td.documentsNo,
                    Documenturl = td.documenturl,
                    isActive = td.isActive,
                    CompanyID = tenantDto.companyID
                }).ToList();
            }

            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<bool> UpdateTenant(int tenantId, TenantCreateDto tenantDto)
        {
            var tenant = await _context.Tenants
                .Include(t => t.TenantDocuments)
                .FirstOrDefaultAsync(t => t.tenantID == tenantId);

            if (tenant == null) return false;

            tenant.tenantName = tenantDto.tenantName;
            tenant.unitID = tenantDto.unitID;
            tenant.propID = tenantDto.propID;
            tenant.companyID = tenantDto.companyID;
            tenant.phoneNumber = tenantDto.phoneNumber;
            tenant.email = tenantDto.email;
            tenant.emergencyContactNo = tenantDto.emergencyContactNo;
            tenant.concessionper = tenantDto.concessionper;
            tenant.note = tenantDto.note;
            tenant.address = tenantDto.address;
            tenant.isActive = tenantDto.isActive;

            if (tenantDto.TenantDocuments != null)
            {
                foreach (var tdDto in tenantDto.TenantDocuments)
                {
                    var existingDoc = tenant.TenantDocuments.FirstOrDefault(d => d.DocTypeId == tdDto.docTypeId);
                    if (existingDoc != null)
                    {
                        existingDoc.DocumentsNo = tdDto.documentsNo;
                        existingDoc.Documenturl = tdDto.documenturl;
                        existingDoc.isActive = tdDto.isActive;
                    }
                    else
                    {
                        tenant.TenantDocuments.Add(new XRS_TenantDocuments
                        {
                            DocTypeId = tdDto.docTypeId,
                            DocumentsNo = tdDto.documentsNo,
                            Documenturl = tdDto.documenturl,
                            isActive = tdDto.isActive,
                            CompanyID = tenantDto.companyID,
                            TenantID = tenantId
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<string, string>> UploadFilesAsync(List<IFormFile> files)
        {
            var uploaded = new Dictionary<string, string>();
            string folderName = "Rental/images";
            string uploadFolderPath = $"{_ftp.BaseUrl}/{folderName}";
            try
            {
                var dirRequest = (FtpWebRequest)WebRequest.Create(uploadFolderPath);
                dirRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                dirRequest.Credentials = new NetworkCredential(_ftp.Username, _ftp.Password);
                using var dirResponse = (FtpWebResponse)await dirRequest.GetResponseAsync();
            }
            catch (WebException ex)
            {
                var response = (FtpWebResponse)ex.Response;
                if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    throw;
                }
            }

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file.FileName);
                string uniqueFileName = $"{Guid.NewGuid()}{extension}";

                string uploadPath = $"{uploadFolderPath}/{uniqueFileName}";
                var request = (FtpWebRequest)WebRequest.Create(uploadPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftp.Username, _ftp.Password);
                request.UseBinary = true;
                request.UsePassive = true;

                using var stream = await request.GetRequestStreamAsync();
                await file.CopyToAsync(stream);
                string publicUrl = $"{_ftp.PublicBaseUrl}/{uniqueFileName}";
                uploaded[file.FileName] = publicUrl;
            }

            return uploaded;
        }

        public async Task<bool> DeleteTenant(int id)
        {
            var tenants = await _context.Tenants.FirstOrDefaultAsync(u => u.tenantID == id);
            if (tenants == null) return false;
            tenants.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
