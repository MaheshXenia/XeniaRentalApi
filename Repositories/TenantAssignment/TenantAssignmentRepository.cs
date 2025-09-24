using Microsoft.EntityFrameworkCore;
using XeniaRentalApi.Dtos;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.Repositories.TenantAssignment
{
    public class TenantAssignmentRepository : ITenantAssignmentRepository
    {
        private readonly ApplicationDbContext _context;
        public TenantAssignmentRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<TenantAssignmentGetDto>> GetByCompanyIdAsync(int companyId)
        {
            var assignments = await _context.TenantAssignemnts
                .Include(t => t.Properties)
                .Include(t => t.Unit)
                .Include(t => t.Tenant)
                    .ThenInclude(tenant => tenant.TenantDocuments)
                        .ThenInclude(td => td.Documents)
                .Where(t => t.companyID == companyId)
                .AsNoTracking()
                .ToListAsync();

     
            var result = assignments.Select(t => new TenantAssignmentGetDto
            {
                tenantAssignId = t.tenantAssignId,
                propID = t.propID,
                PropName = t.Properties?.propertyName,
                unitID = t.unitID,
                UnitName = t.Unit?.UnitName,
                tenantID = t.tenantID,
                TenantName = t.Tenant?.tenantName,
                TenantContactNo = t.Tenant?.phoneNumber,
                rentAmt = t.rentAmt,
                rentConcession = t.rentConcession,
                messConcession = t.messConcession,
                agreementStartDate = t.agreementStartDate,
                agreementEndDate = t.agreementEndDate,
                isActive = t.isActive,
                isClosure = t.isClosure,
                notes = t.notes,

       
                Documents = t.Tenant?.TenantDocuments?
                    .Select(td => new TenantDocumentDto
                    {
                        TenantID = td.TenantID,
                        DocTypeId = td.DocTypeId,
                        CompanyID = td.CompanyID,
                        DocumentsNo = td.DocumentsNo,
                        DocumentUrl = td.Documenturl,
                        IsActive = td.isActive,
                        DocumentName = td.Documents?.docName ?? td.DocumentName,
                        IsAlphaNumeric = td.Documents?.isAlphanumeric ?? td.IsAlphaNumeric ?? false,
                        IsMandatory = td.Documents?.isMandatory ?? td.IsMandatory ?? false,
                        IsExpiry = td.Documents?.isExpiry ?? td.IsExpiry ?? false,
                        DocPurpose = td.Documents?.docPurpose ?? td.DocPurpose,
                        ExpiryDate = td.Documents?.ExpiryDate ?? td.ExpiryDate
                    })
                    .ToList() ?? new List<TenantDocumentDto>()
            }).ToList();

            return result;
        }

        public async Task<TenantAssignmentGetDto?> GetByIdAsync(int tenantAssignId)
        {
            var assignment = await _context.TenantAssignemnts
                .Include(t => t.Properties)
                .Include(t => t.Unit)
                .Include(t => t.Tenant)
                    .ThenInclude(tenant => tenant.TenantDocuments)
                        .ThenInclude(td => td.Documents)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.tenantAssignId == tenantAssignId);

            if (assignment == null)
                return null;

            return new TenantAssignmentGetDto
            {
                tenantAssignId = assignment.tenantAssignId,
                propID = assignment.propID,
                PropName = assignment.Properties?.propertyName,
                unitID = assignment.unitID,
                UnitName = assignment.Unit?.UnitName,
                tenantID = assignment.tenantID,
                TenantName = assignment.Tenant?.tenantName,
                TenantContactNo = assignment.Tenant?.phoneNumber,
                rentAmt = assignment.rentAmt,
                rentConcession = assignment.rentConcession,
                messConcession = assignment.messConcession,
                agreementStartDate = assignment.agreementStartDate,
                agreementEndDate = assignment.agreementEndDate,
                isActive = assignment.isActive,
                isClosure = assignment.isClosure,
                notes = assignment.notes,

                Documents = assignment.Tenant?.TenantDocuments?
                    .Select(td => new TenantDocumentDto
                    {
                        TenantID = td.TenantID,
                        DocTypeId = td.DocTypeId,
                        CompanyID = td.CompanyID,
                        DocumentsNo = td.DocumentsNo,
                        DocumentUrl = td.Documenturl,
                        IsActive = td.isActive,
                        DocumentName = td.Documents?.docName ?? td.DocumentName,
                        IsAlphaNumeric = td.Documents?.isAlphanumeric ?? td.IsAlphaNumeric ?? false,
                        IsMandatory = td.Documents?.isMandatory ?? td.IsMandatory ?? false,
                        IsExpiry = td.Documents?.isExpiry ?? td.IsExpiry ?? false,
                        DocPurpose = td.Documents?.docPurpose ?? td.DocPurpose,
                        ExpiryDate = td.Documents?.ExpiryDate ?? td.ExpiryDate
                    })
                    .ToList() ?? new List<TenantDocumentDto>()
            };
        }

        public async Task<XRS_TenantAssignment> CreateAsync(TenantAssignmentCreateDto dto)
        {
            var entity = new XRS_TenantAssignment
            {
                propID = dto.propID,
                unitID = dto.unitID,
                tenantID = dto.tenantID,
                companyID = dto.companyID,
                securityAmt = dto.securityAmt,
                isTaxable = dto.isTaxable,
                bedSpaceID = dto.bedSpaceID,
                rentAmt = dto.rentAmt,
                rentConcession = dto.rentConcession,
                messConcession = dto.messConcession,            
                collectionType = dto.collectionType,
                agreementStartDate = dto.agreementStartDate,
                agreementEndDate = dto.agreementEndDate,
                rentCollection = dto.rentCollection,
                escalationPer = dto.escalationPer,
                nextescalationDate = dto.nextescalationDate,
                rentDueDate = dto.rentDueDate,
                notes = dto.notes,
                isActive = true
            };

            _context.TenantAssignemnts.Add(entity);
            await _context.SaveChangesAsync();

            if (dto.Documents != null && dto.Documents.Any())
            {
                var tenantDocs = dto.Documents.Select(doc => new XRS_TenantDocuments
                {
                    TenantID = dto.tenantID, 
                    CompanyID = dto.companyID,
                    DocTypeId = doc.docTypeId,
                    DocumentsNo = doc.documentsNo,
                    Documenturl = doc.documenturl,
                    isActive = doc.isActive
                }).ToList();

                _context.TenantDocuments.AddRange(tenantDocs);
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<bool> UpdateAsync(int tenantAssignId, TenantAssignmentCreateDto dto)
        {
            var entity = await _context.TenantAssignemnts
                .Include(t => t.Tenant)
                    .ThenInclude(tenant => tenant.TenantDocuments)
                .FirstOrDefaultAsync(t => t.tenantAssignId == tenantAssignId);

            if (entity == null) return false;

            entity.securityAmt = dto.securityAmt;
            entity.isTaxable = dto.isTaxable;
            entity.rentAmt = dto.rentAmt;
            entity.rentConcession = dto.rentConcession;
            entity.messConcession = dto.messConcession;
            entity.collectionType = dto.collectionType;
            entity.agreementStartDate = dto.agreementStartDate;
            entity.agreementEndDate = dto.agreementEndDate;
            entity.rentCollection = dto.rentCollection;
            entity.escalationPer = dto.escalationPer;
            entity.nextescalationDate = dto.nextescalationDate;
            entity.rentDueDate = dto.rentDueDate;
            entity.notes = dto.notes;


            if (dto.Documents != null && dto.Documents.Any())
            {
                if (entity.Tenant?.TenantDocuments != null)
                {
                    _context.TenantDocuments.RemoveRange(entity.Tenant.TenantDocuments);
                }

                var newDocs = dto.Documents.Select(doc => new XRS_TenantDocuments
                {
                    TenantID = entity.tenantID,
                    CompanyID = entity.companyID,
                    DocTypeId = doc.docTypeId,
                    DocumentsNo = doc.documentsNo,
                    Documenturl = doc.documenturl,
                    isActive = doc.isActive
                }).ToList();

                _context.TenantDocuments.AddRange(newDocs);
            }

            var saved = await _context.SaveChangesAsync();
            return saved > 0; 
        }

        public async Task<bool> UpdateClosureAsync(int tenantAssignId, TenantClosureCreateDto dto)
        {
            var entity = await _context.TenantAssignemnts
                .FirstOrDefaultAsync(t => t.tenantAssignId == tenantAssignId);

            if (entity == null) return false;

            entity.isClosure = dto.isClosure;
            entity.closureDate = dto.closureDate;
            entity.closureReason = dto.closureReason;

            _context.TenantAssignemnts.Update(entity);

            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> DeleteAsync(int tenantAssignId)
        {
            var entity = await _context.TenantAssignemnts.FindAsync(tenantAssignId);
            if (entity == null) return false;

            _context.TenantAssignemnts.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }


} 

