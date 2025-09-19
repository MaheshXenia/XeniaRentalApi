using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Product;
using static System.Net.WebRequestMethods;

namespace XeniaRentalApi.Repositories.TenantDocuments
{
    public class TenantDocumentRepository:ITenantDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly FtpSettings _ftp;
        public TenantDocumentRepository(ApplicationDbContext context, IOptions<FtpSettings> ftpOptions)
        {
            _context = context;
            _ftp = ftpOptions.Value;
        }

        public async Task<IEnumerable<Models.TenantDocuments>> GetTenantDocuments()
        {

            return await _context.TenantDocuments.Where(u => u.isActive == true).ToListAsync();

        }


        public async Task<PagedResultDto<Models.TenantDocuments>> GetTenantDocumentsByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.TenantDocuments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.CompanyID.Equals(companyId)); // Adjust property as needed
            }

            var totalRecords = await query.CountAsync();

            var items = await query
               .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
              .Select(u => new Models.TenantDocuments
              {
                  TenantDocId = u.TenantDocId,
                  TenantID = u.TenantID,
                  CompanyID = companyId,
                  isActive = true,
                  Docmenturl = u.Docmenturl,
                  DocTypeId = u.DocTypeId,
                  DocumentsNo = u.DocumentsNo,

              }).ToListAsync();
            return new PagedResultDto<Models.TenantDocuments>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };


        }

        public async Task<IEnumerable<Models.TenantDocuments>> GetTenantDocumentsbyId(int documentId)
        {

            return await _context.TenantDocuments
                .Where(u => u.TenantDocId == documentId)
                 .ToListAsync();

        }

        public async Task<Models.TenantDocuments> CreateTenantDocuments(DTOs.CreateTenantDocuments documentsDTO)
        {

            var documents = new Models.TenantDocuments
            {
               DocTypeId = documentsDTO.DocTypeId,
               TenantID = documentsDTO.TenantID,
               Docmenturl=documentsDTO.Documenturl,
               CompanyID = documentsDTO.CompanyID,
               DocumentsNo = documentsDTO.DocumentsNo,
               isActive = documentsDTO.isActive

            };
            await _context.TenantDocuments.AddAsync(documents);
            await _context.SaveChangesAsync();
            return documents;

        }

        public async Task<bool> DeleteTenantDocument(int id)
        {
            var bedspacesettings = await _context.TenantDocuments.FirstOrDefaultAsync(u => u.TenantDocId == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.isActive = false;
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<Models.TenantDocuments>> GetTenantDocumentsAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.TenantDocuments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.DocumentsNo.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.TenantDocuments
                {
                   DocTypeId = u.DocTypeId,
                   CompanyID=u.CompanyID,
                   DocumentsNo = u.DocumentsNo,
                   Docmenturl = u.Docmenturl,
                   isActive = u.isActive
                })
                .ToListAsync();

            return new PagedResultDto<Models.TenantDocuments>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
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

        public async Task<bool> UpDateTenantDocument(int id, Models.TenantDocuments documents)
        {
            var updateassignemnts = await _context.TenantDocuments.FirstOrDefaultAsync(u => u.TenantDocId == id);
            if (updateassignemnts == null) return false;

            updateassignemnts.DocumentsNo = documents.DocumentsNo;
            updateassignemnts.TenantDocId = documents.TenantDocId;
            updateassignemnts.Docmenturl = documents.Docmenturl;
            updateassignemnts.TenantID = documents.TenantID;
            updateassignemnts.CompanyID = documents.CompanyID;
            updateassignemnts.DocTypeId= documents.DocTypeId;
            updateassignemnts.isActive = documents.isActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
