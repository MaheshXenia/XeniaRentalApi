using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.Documents
{
    public class DocumentRepository:IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<XRS_Documents>> GetDocuments(int companyId)
        {
            return await _context.Documents
                .Where(u => u.companyID == companyId)
                .Select(u => new Models.XRS_Documents
                {
                    docTypeId = u.docTypeId,
                    docPurpose = u.docPurpose,
                    docName = u.docName,
                    propID = u.propID,
                    isActive = u.isActive,
                    isMandatory = u.isMandatory,
                    isAlphanumeric = u.isAlphanumeric,
                    isExpiry = u.isExpiry,
                    ExpiryDate = u.ExpiryDate
                })
                .ToListAsync();
        }



        public async Task<PagedResultDto<XRS_Documents>> GetDocumentsCompanyId(int companyId, string? search = null,int pageNumber = 1,int pageSize = 10)
        {
            var query = _context.Documents.AsQueryable();

            query = query.Where(u => u.companyID == companyId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(u => u.docName.ToLower().Contains(lowerSearch));
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .GroupJoin(
                    _context.Properties,
                    doc => doc.propID,
                    prop => prop.PropID,
                    (doc, props) => new { doc, prop = props.FirstOrDefault() }
                )
                .OrderBy(u => u.doc.docName) 
                .Take(pageSize)
                .Select(u => new Models.XRS_Documents
                {
                    docTypeId = u.doc.docTypeId,
                    docPurpose = u.doc.docPurpose,
                    docName = u.doc.docName,
                    propID = u.doc.propID,
                    isActive = u.doc.isActive,
                    isMandatory = u.doc.isMandatory,
                    isAlphanumeric = u.doc.isAlphanumeric,
                    isExpiry = u.doc.isExpiry,
                    propName = u.prop != null ? u.prop.propertyName : null,
                    companyID = u.doc.companyID,
                    ExpiryDate = u.doc.ExpiryDate
                })
                .ToListAsync();

            return new PagedResultDto<Models.XRS_Documents>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }


        public async Task<IEnumerable<XRS_Documents>> GetDocumentsbyId(int documentId)
        {

            return await _context.Documents
                .Where(u => u.docTypeId == documentId)
                 .GroupJoin(
                 _context.Properties,
                 doc => doc.propID,
                prop => prop.PropID,
                (doc, props) => new { doc, prop = props.FirstOrDefault() }
                )
                .Select(u => new Models.XRS_Documents
                {
                    docTypeId = u.doc.docTypeId,
                    docPurpose = u.doc.docPurpose,
                    docName = u.doc.docName,
                    propID = u.doc.propID,
                    propName = u.prop != null ? u.prop.propertyName : null,
                    isActive = u.doc.isActive,
                    isMandatory = u.doc.isMandatory,
                    isAlphanumeric = u.doc.isAlphanumeric,
                    isExpiry = u.doc.isExpiry,
                    ExpiryDate = u.doc.ExpiryDate

                }).ToListAsync();

        }

        public async Task<XRS_Documents> CreateDocuments(XRS_Documents dtoDocuments)
        {

            var documents = new Models.XRS_Documents
            {
                docName = dtoDocuments.docName,
                docPurpose = dtoDocuments.docPurpose,
                companyID = dtoDocuments.companyID,
                propID = dtoDocuments.propID,
                isActive = dtoDocuments.isActive,
                isMandatory = dtoDocuments.isMandatory,
                isAlphanumeric = dtoDocuments.isAlphanumeric,
                isExpiry = dtoDocuments.isExpiry,
                ExpiryDate= dtoDocuments.ExpiryDate


            };
           


            await _context.Documents.AddAsync(documents);
            await _context.SaveChangesAsync();
            return documents;

        }

        public async Task<bool> DeleteDocumentType(int id)
        {
            var documents = await _context.Documents.FirstOrDefaultAsync(u => u.companyID == id);
            if (documents == null) return false;
            documents.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDocuments(int id, XRS_Documents documents)
        {
            var updatedDocuments = await _context.Documents.FirstOrDefaultAsync(u => u.docTypeId == id);
            if (updatedDocuments == null) return false;

            updatedDocuments.docName = documents.docName ?? documents.docName;
            updatedDocuments.companyID = documents.companyID;
            updatedDocuments.docPurpose = documents.docPurpose;
            updatedDocuments.propID = documents.propID;
            updatedDocuments.isExpiry = documents.isExpiry;
            updatedDocuments.isAlphanumeric = documents.isAlphanumeric;
            updatedDocuments.isMandatory = documents.isMandatory;
            updatedDocuments.isActive = documents.isActive;
            updatedDocuments.ExpiryDate = documents.ExpiryDate;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<XRS_Documents>> GetDocumentsAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Documents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.docName.Contains(search)); 

            }

            var totalRecords = await query.CountAsync();

            var items = await query            
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new XRS_Documents
                {
                    companyID = u.companyID,
                    docName = u.docName,
                    docPurpose = u.docPurpose,
                    propID = u.propID,
                    isExpiry = u.isExpiry,
                    isActive = u.isActive,
                    isAlphanumeric= u.isAlphanumeric,
                    isMandatory = u.isMandatory,
                    propName = u.Properties != null ? u.Properties.propertyName : null,

                })
                .ToListAsync();

            return new PagedResultDto<XRS_Documents>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
