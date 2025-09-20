using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Net;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Product;
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

        public async Task<IEnumerable<Models.Tenant>> GetTenants()
        {
            return await _context.Tenants
                   .GroupJoin(_context.Properties,
        t => t.propID,
        p => p.PropID,
        (t, props) => new { t, props })
    .SelectMany(
        tp => tp.props.DefaultIfEmpty(),
        (tp, prop) => new { Tenant = tp.t, Property = prop }
    )
    .GroupJoin(_context.Units,
        tp => tp.Tenant.unitID,
        u => u.UnitId,
        (tp, units) => new { tp.Tenant, tp.Property, units })
    .SelectMany(
        tpu => tpu.units.DefaultIfEmpty(),
        (tpu, unit) => new { tpu.Tenant, tpu.Property, Unit = unit })
                .Select(u => new Models.Tenant
                {
                    tenantID = u.Tenant.tenantID,
                    tenantName = u.Tenant.tenantName,
                    propID = u.Tenant.propID,
                    companyID = u.Tenant.companyID,
                    email = u.Tenant.email,
                    PropName = u.Property != null ? u.Property.propertyName : null,
                    UnitName = u.Unit != null ? u.Unit.UnitName: null,
                    isActive = u.Tenant.isActive,
                    address = u.Tenant.address,
                    concessionper = u.Tenant.concessionper,
                    emergencyContactNo = u.Tenant.emergencyContactNo,
                    note = u.Tenant.note,
                    unitID = u.Tenant.unitID,
                    phoneNumber = u.Tenant.phoneNumber,

                }).ToListAsync();
        }


        public async Task<PagedResultDto<Models.Tenant>> GetTenantsByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.Tenants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
             .GroupJoin(_context.Properties,
        t => t.propID,
        p => p.PropID,
        (t, props) => new { t, props })
    .SelectMany(
        tp => tp.props.DefaultIfEmpty(),
        (tp, prop) => new { Tenant = tp.t, Property = prop }
    )
    .GroupJoin(_context.Units,
        tp => tp.Tenant.unitID,
        u => u.UnitId,
        (tp, units) => new { tp.Tenant, tp.Property, units })
    .SelectMany(
        tpu => tpu.units.DefaultIfEmpty(),
        (tpu, unit) => new { tpu.Tenant, tpu.Property, Unit = unit })

    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .Select(x => new Models.Tenant
    {
        tenantID = x.Tenant.tenantID,
        tenantName = x.Tenant.tenantName,
        propID = x.Tenant.propID,
        companyID = x.Tenant.companyID,
        email = x.Tenant.email,
        PropName = x.Property != null ? x.Property.propertyName : null,
        UnitName = x.Unit!= null? x.Unit.UnitName: null,
        isActive = x.Tenant.isActive,
        address = x.Tenant.address,
        concessionper = x.Tenant.concessionper,
        emergencyContactNo = x.Tenant.emergencyContactNo,
        note = x.Tenant.note,
        unitID = x.Tenant.unitID,
        phoneNumber = x.Tenant.phoneNumber,
    })
    .ToListAsync();

            return new PagedResultDto<Models.Tenant>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

        }

        public async Task<TenantDocumentGetDTO> GetTenantsbyId(int tenantId)
        {

            var tenant =  await _context.Tenants.Where(u => u.tenantID == tenantId)
                  .GroupJoin(_context.Properties,
        t => t.propID,
        p => p.PropID,
        (t, props) => new { t, props })
    .SelectMany(
        tp => tp.props.DefaultIfEmpty(),
        (tp, prop) => new { Tenant = tp.t, Property = prop }
    )
    .GroupJoin(_context.Units,
        tp => tp.Tenant.unitID,
        u => u.UnitId,
        (tp, units) => new { tp.Tenant, tp.Property, units })
    .SelectMany(
        tpu => tpu.units.DefaultIfEmpty(),
        (tpu, unit) => new { tpu.Tenant, tpu.Property, Unit = unit })
                 .Select(u => new Models.Tenant
                 {
                     tenantID = u.Tenant.tenantID,
                     tenantName = u.Tenant.tenantName,
                     propID = u.Tenant.propID,
                     companyID = u.Tenant.companyID,
                     email = u.Tenant.email,
                     PropName = u.Property != null ? u.Property.propertyName : null,
                     UnitName = u.Unit != null ? u.Unit.UnitName : null,
                     isActive = u.Tenant.isActive,
                     address = u.Tenant.address,
                     concessionper = u.Tenant.concessionper,
                     emergencyContactNo = u.Tenant.emergencyContactNo,
                     note = u.Tenant.note,
                     unitID = u.Tenant.unitID,
                     phoneNumber = u.Tenant.phoneNumber,
                     
                 }).FirstOrDefaultAsync();

            if (tenant == null)
            {
                return new TenantDocumentGetDTO
                {
                    Tenant = null,
                    Documents = new List<Models.TenantDocuments>()
                };
            }


            var assignmentDocs = await _context.TenantDocuments
                .Where(u => u.TenantID == tenant.tenantID)
                .Include(u => u.Documents)
                .Select(u => new Models.TenantDocuments
                {
                    TenantID = u.TenantID,
                    DocTypeId = u.DocTypeId,
                    DocumentsNo = u.DocumentsNo,
                    Docmenturl = u.Docmenturl,
                    isActive = u.isActive,
                    CompanyID = u.CompanyID,
                    DocumentName = u.Documents.docName,
                    IsAlphaNumeric = u.Documents.isAlphanumeric,
                    IsMandatory = u.Documents.isMandatory,
                   IsExpiry = u.Documents.isExpiry,
                   DocPurpose = u.Documents.docPurpose,
                   ExpiryDate = u.Documents.ExpiryDate

                })
                .ToListAsync();


            TenantDocumentGetDTO dtotenantDocs = new TenantDocumentGetDTO();
            dtotenantDocs.Tenant = tenant;
            dtotenantDocs.Documents = assignmentDocs;

            return dtotenantDocs;

        }

        public async Task<Models.Tenant> CreateTenant(DTOs.CreateTenant dtoTenant)
        {
            var tenant = new  Models.Tenant
            {
                unitID = dtoTenant.unitID,
                propID = dtoTenant.propID,
                companyID = dtoTenant.companyID,
                tenantName = dtoTenant.tenantName,
                phoneNumber = dtoTenant.phoneNumber,
                email = dtoTenant.email,
                emergencyContactNo = dtoTenant.emergencyContactNo,
                concessionper = dtoTenant.concessionper,
                note = dtoTenant.note,
                address = dtoTenant.address,
                isActive = dtoTenant.isActive,
            };

            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();
            return tenant;

        }

        public async Task<Models.Tenant> AddTenantWithDocumentsAsync(TenantWithDocumentsDto dto)
        {
            var tenant = new Models.Tenant
            {
                unitID = dto.Tenant.unitID,
                propID = dto.Tenant.propID,
                companyID = dto.Tenant.companyID,
                tenantName = dto.Tenant.tenantName,
                phoneNumber = dto.Tenant.phoneNumber,
                email = dto.Tenant.email,
                emergencyContactNo = dto.Tenant.emergencyContactNo,
                concessionper = dto.Tenant.concessionper,
                note = dto.Tenant.note,
                address = dto.Tenant.address,
                isActive = dto.Tenant.isActive

            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync(); // Get TenantId

            foreach (var doc in dto.Documents)
            {
                
                var tenantdoc = new Models.TenantDocuments
                {
                    CompanyID = doc.CompanyID,
                    DocumentsNo = doc.DocumentsNo,
                    TenantID = tenant.tenantID,
                    DocTypeId = doc.DocTypeId,
                    Docmenturl = doc.Documenturl,
                    isActive = doc.isActive

                };

                 _context.TenantDocuments.Add(tenantdoc);
                await _context.SaveChangesAsync();

            }

            return tenant;
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
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpDateTenant(int id, Models.Tenant tenant)
        {
            var updateTenant = await _context.Tenants.FirstOrDefaultAsync(u => u.tenantID == id);
            if (updateTenant == null) return false;

            updateTenant.tenantName = tenant.tenantName;
            updateTenant.companyID = tenant.companyID;
            updateTenant.email = tenant.email;
            updateTenant.address = tenant.address;
            updateTenant.unitID = tenant.unitID;
            updateTenant.concessionper = tenant.concessionper;
            updateTenant.phoneNumber = tenant.phoneNumber;
            updateTenant.propID = tenant.propID;
            updateTenant.emergencyContactNo = tenant.emergencyContactNo;
            updateTenant.note = tenant.note;
            updateTenant.isActive = tenant.isActive;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpDateTenantDocuments(DTOs.TenantDocumentDTO documentDTO)
        {
            var updateTenant = await _context.Tenants
       .FirstOrDefaultAsync(t => t.tenantID == documentDTO.Tenant.tenantID);

            if (updateTenant == null) return false;

            updateTenant.tenantName = documentDTO.Tenant.tenantName;
            updateTenant.companyID = documentDTO.Tenant.companyID;
            updateTenant.email = documentDTO.Tenant.email;
            updateTenant.address = documentDTO.Tenant.address;
            updateTenant.unitID = documentDTO.Tenant.unitID;
            updateTenant.concessionper = documentDTO.Tenant.concessionper;
            updateTenant.phoneNumber = documentDTO.Tenant.phoneNumber;
            updateTenant.propID = documentDTO.Tenant.propID;
            updateTenant.emergencyContactNo = documentDTO.Tenant.emergencyContactNo;
            updateTenant.note = documentDTO.Tenant.note;
            updateTenant.isActive = documentDTO.Tenant.isActive;
            await _context.SaveChangesAsync();

            foreach (var docDto in documentDTO.Documents)
            {
                if (docDto.tenantDocId > 0)
                {
                    // Update existing document
                    var existingDoc = await _context.TenantDocuments
                        .Where(d => d.TenantDocId == docDto.tenantDocId).FirstOrDefaultAsync();

                    if (existingDoc != null)
                    {
                        existingDoc.Docmenturl = docDto.Documenturl;
                        existingDoc.DocumentsNo = docDto.DocumentsNo;
                        existingDoc.DocTypeId = docDto.DocTypeId;
                        existingDoc.isActive = docDto.isActive;
                        existingDoc.CompanyID = docDto.CompanyID;
                        // ... other fields
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Insert new document
                    var tenantdoc = new Models.TenantDocuments
                    {
                        CompanyID = docDto.CompanyID,
                        DocumentsNo = docDto.DocumentsNo,
                        TenantID = updateTenant.tenantID,
                        DocTypeId = docDto.DocTypeId,
                        Docmenturl = docDto.Documenturl,
                        isActive = docDto.isActive

                    };

                    _context.TenantDocuments.Add(tenantdoc);
                    await _context.SaveChangesAsync();
                }
            }

            
            return true;
        }

        public async Task<PagedResultDto<Models.Tenant>> GetTenantAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Tenants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.tenantName.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.Tenant
                {
                    tenantName = u.tenantName,
                    email = u.email,
                    emergencyContactNo=u.emergencyContactNo,
                    isActive = u.isActive,
                    

                })
                .ToListAsync();

            return new PagedResultDto<Models.Tenant>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        
    }
}
