using Microsoft.EntityFrameworkCore;
using Stripe;
using System.ComponentModel.Design;
using XeniaRentalApi.DTOs;
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

        public async Task<IEnumerable<Models.TenantAssignemnt>> GetTenantAssignments()
        {

            return await _context.TenantAssignemnts
                  .GroupJoin(_context.Properties,
                    ta => ta.propID,
                    p => p.PropID,
                    (ta, props) => new { ta, props })
                .SelectMany(
                    tap => tap.props.DefaultIfEmpty(),
                    (tap, prop) => new { tap.ta, Property = prop }
                )
                .GroupJoin(_context.Units,
                    tap => tap.ta.unitID,
                    u => u.UnitId,
                    (tap, units) => new { tap.ta, tap.Property, units })
                .SelectMany(
                    tapu => tapu.units.DefaultIfEmpty(),
                    (tapu, unit) => new { tapu.ta, tapu.Property, Unit = unit }
                )
                .GroupJoin(_context.Tenants,
                    tapu => tapu.ta.tenantID,
                    t => t.tenantID,
                    (tapu, tenants) => new { tapu.ta, tapu.Property, tapu.Unit, tenants })
                .SelectMany(
                    full => full.tenants.DefaultIfEmpty(),
                    (full, tenant) => new { full.ta, full.Property, full.Unit, Tenant = tenant }
                )
               .Select(u => new Models.TenantAssignemnt
               {
                   tenantAssignId = u.ta.tenantAssignId,
                   tenantID = u.ta.tenantID,
                   propID = u.ta.propID,
                   unitID = u.ta.unitID,
                   companyID = u.ta.companyID,
                   agreementEndDate = u.ta.agreementEndDate,
                   agreementStartDate = u.ta.agreementStartDate,
                   securityAmt = u.ta.securityAmt,
                   rentAmt = u.ta.rentAmt,
                   collectionType = u.ta.collectionType,
                   rentCollection = u.ta.rentCollection,
                   escalationPer = u.ta.escalationPer,
                   nextescalationDate = u.ta.nextescalationDate,
                   closureDate = u.ta.closureDate,
                   closureReason = u.ta.closureReason,
                   isClosure = u.ta.isClosure,
                   PropName = u.Property != null ? u.Property.propertyName : null,
                   UnitName = u.Unit != null ? u.Unit.UnitName : null,
                   TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                   isActive = u.ta.isActive,
                   notes = u.ta.notes,
                   refundAmount = u.ta.refundAmount,
                   TenantContactNo = u.Tenant != null ? u.Tenant.emergencyContactNo : null
               }).ToListAsync();


        }

        public async Task<IEnumerable<Models.TenantAssignemnt>> GetAllCloseAgreemnts()
        {
            return await _context.TenantAssignemnts.Where(u => u.isClosure == true)
                .Select(u => new Models.TenantAssignemnt
                {
                    tenantAssignId = u.tenantAssignId,
                    tenantID = u.tenantID,
                    propID = u.propID,
                    unitID = u.unitID,
                    companyID = u.companyID,
                    agreementEndDate = u.agreementEndDate,
                    agreementStartDate = u.agreementStartDate,
                    securityAmt = u.securityAmt,
                    rentAmt = u.rentAmt,
                    collectionType = u.collectionType,
                    rentCollection = u.rentCollection,
                    escalationPer = u.escalationPer,
                    nextescalationDate = u.nextescalationDate,
                    closureDate = u.closureDate,
                    closureReason = u.closureReason,
                    isClosure = u.isClosure,
                    PropName = u.Properties != null ? u.Properties.propertyName : null,
                    UnitName = u.Unit != null ? u.Unit.UnitName : null,
                    TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                    isActive = u.isActive,
                    notes = u.notes,
                    refundAmount = u.refundAmount,
                }).ToListAsync();

        }

        public async Task<IEnumerable<Models.TenantAssignemnt>> GetAllCloseAgreemntsByParams(DateTime startDate, DateTime endDate, int propId, int unitId)
        {
            return await _context.TenantAssignemnts.Where(u => u.propID == propId && u.unitID == unitId && u.isClosure == true && u.closureDate >= startDate && u.closureDate <= endDate)
             .Select(u => new Models.TenantAssignemnt
             {
                 tenantAssignId = u.tenantAssignId,
                 tenantID = u.tenantID,
                 propID = u.propID,
                 unitID = u.unitID,
                 companyID = u.companyID,
                 agreementEndDate = u.agreementEndDate,
                 agreementStartDate = u.agreementStartDate,
                 securityAmt = u.securityAmt,
                 rentAmt = u.rentAmt,
                 collectionType = u.collectionType,
                 rentCollection = u.rentCollection,
                 escalationPer = u.escalationPer,
                 nextescalationDate = u.nextescalationDate,
                 closureDate = u.closureDate,
                 closureReason = u.closureReason,
                 isClosure = u.isClosure,
                 PropName = u.Properties != null ? u.Properties.propertyName : null,
                 UnitName = u.Unit != null ? u.Unit.UnitName : null,
                 TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                 isActive = u.isActive,
                 notes = u.notes,
                 refundAmount = u.refundAmount,
             }).ToListAsync();

        }


        public async Task<PagedResultDto<Models.TenantAssignemnt>> GetTenantAssignmentsByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.TenantAssignemnts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID == companyId);
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .GroupJoin(_context.Properties,
                    ta => ta.propID,
                    p => p.PropID,
                    (ta, props) => new { ta, props })
                .SelectMany(
                    tap => tap.props.DefaultIfEmpty(),
                    (tap, prop) => new { tap.ta, Property = prop }
                )
                .GroupJoin(_context.Units,
                    tap => tap.ta.unitID,
                    u => u.UnitId,
                    (tap, units) => new { tap.ta, tap.Property, units })
                .SelectMany(
                    tapu => tapu.units.DefaultIfEmpty(),
                    (tapu, unit) => new { tapu.ta, tapu.Property, Unit = unit }
                )
                .GroupJoin(_context.Tenants,
                    tapu => tapu.ta.tenantID,
                    t => t.tenantID,
                    (tapu, tenants) => new { tapu.ta, tapu.Property, tapu.Unit, tenants })
                .SelectMany(
                    full => full.tenants.DefaultIfEmpty(),
                    (full, tenant) => new { full.ta, full.Property, full.Unit, Tenant = tenant }
                )
                .OrderBy(x => x.ta.agreementStartDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new Models.TenantAssignemnt
                {
                    tenantAssignId = x.ta.tenantAssignId,
                    tenantID = x.ta.tenantID,
                    propID = x.ta.propID,
                    unitID = x.ta.unitID,
                    companyID = x.ta.companyID,
                    agreementEndDate = x.ta.agreementEndDate,
                    agreementStartDate = x.ta.agreementStartDate,
                    securityAmt = x.ta.securityAmt,
                    rentAmt = x.ta.rentAmt,
                    collectionType = x.ta.collectionType,
                    rentCollection = x.ta.rentCollection,
                    escalationPer = x.ta.escalationPer,
                    nextescalationDate = x.ta.nextescalationDate,
                    closureDate = x.ta.closureDate,
                    closureReason = x.ta.closureReason,
                    isClosure = x.ta.isClosure,
                    PropName = x.Property != null ? x.Property.propertyName : null,
                    UnitName = x.Unit != null ? x.Unit.UnitName : null,
                    TenantName = x.Tenant != null ? x.Tenant.tenantName : null,
                    isActive = x.ta.isActive,
                    notes = x.ta.notes,
                    refundAmount = x.ta.refundAmount,
                    TenantContactNo = x.Tenant != null ? x.Tenant.emergencyContactNo : null
                })
                .ToListAsync();

            return new PagedResultDto<Models.TenantAssignemnt>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };



        }

        public async Task<TenantAssignmentDocumentGetDTO> GetTenantAssignemntsbyId(int documassignmentId)
        {

           var assignment = await _context.TenantAssignemnts
                .Where(u => u.tenantAssignId == documassignmentId)
                .GroupJoin(_context.Properties,
                    ta => ta.propID,
                    p => p.PropID,
                    (ta, props) => new { ta, props })
                .SelectMany(
                    tap => tap.props.DefaultIfEmpty(),
                    (tap, prop) => new { tap.ta, Property = prop }
                )
                .GroupJoin(_context.Units,
                    tap => tap.ta.unitID,
                    u => u.UnitId,
                    (tap, units) => new { tap.ta, tap.Property, units })
                .SelectMany(
                    tapu => tapu.units.DefaultIfEmpty(),
                    (tapu, unit) => new { tapu.ta, tapu.Property, Unit = unit }
                )
                .GroupJoin(_context.Tenants,
                    tapu => tapu.ta.tenantID,
                    t => t.tenantID,
                    (tapu, tenants) => new { tapu.ta, tapu.Property, tapu.Unit, tenants })
                .SelectMany(
                    full => full.tenants.DefaultIfEmpty(),
                    (full, tenant) => new { full.ta, full.Property, full.Unit, Tenant = tenant }
                )
                .Select(u => new DTOs.TenantAssignmentDTO
                {
                    tenantAssignId = u.ta.tenantAssignId,
                    tenantID = u.ta.tenantID,
                    propID = u.ta.propID,
                    unitID = u.ta.unitID,
                    companyID = u.ta.companyID,
                    agreementEndDate = u.ta.agreementEndDate,
                    agreementStartDate = u.ta.agreementStartDate,
                    securityAmt = u.ta.securityAmt,
                    rentAmt = u.ta.rentAmt,
                    collectionType = u.ta.collectionType,
                    rentCollection = u.ta.rentCollection,
                    escalationPer = u.ta.escalationPer,
                    nextescalationDate = u.ta.nextescalationDate,
                    PropName = u.Property != null ? u.Property.propertyName : null,
                    UnitName = u.Unit != null ? u.Unit.UnitName : null,
                    TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                    isActive = u.ta.isActive,
                    notes = u.ta.notes,
                    TenantContactNo = u.Tenant != null ? u.Tenant.emergencyContactNo : null
                }).FirstOrDefaultAsync();

            if (assignment == null)
            {
                return new TenantAssignmentDocumentGetDTO
                {
                    Tenant = null,
                    Documents = new List<Models.TenantDocuments>()
                };
            }

            var assignmentDocs = await _context.TenantDocuments
               .Where(u => u.TenantID == assignment.tenantID)
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


            TenantAssignmentDocumentGetDTO dtotenantDocs = new TenantAssignmentDocumentGetDTO();
            dtotenantDocs.Tenant = assignment;
            dtotenantDocs.Documents = assignmentDocs;
            return dtotenantDocs;
        }

        public async Task<Models.TenantAssignemnt> CreateTenantAssignments(DTOs.TenantAssignment dtoAssignments)
        {

            var assignments = new Models.TenantAssignemnt()
            {
                propID = dtoAssignments.propID,
                unitID = dtoAssignments.unitID,
                tenantID = dtoAssignments.tenantID,
                companyID = dtoAssignments.companyID,
                securityAmt = dtoAssignments.securityAmt,
                rentAmt = dtoAssignments.rentAmt,
                collectionType = dtoAssignments.collectionType,
                rentCollection = dtoAssignments.rentCollection,
                agreementEndDate = dtoAssignments.agreementEndDate,
                agreementStartDate = dtoAssignments.agreementStartDate,
                escalationPer = dtoAssignments.escalationPer,
                nextescalationDate = dtoAssignments.nextescalationDate,
                isActive = dtoAssignments.isActive,
                closureDate = dtoAssignments.closureDate,
                closureReason = dtoAssignments.closureReason,
                isClosure = dtoAssignments.isClosure,
                refundAmount = dtoAssignments.refundAmount,
                notes = dtoAssignments.notes
            };

            await _context.TenantAssignemnts.AddAsync(assignments);
            await _context.SaveChangesAsync();
            return assignments;

        }

        public async Task<bool> DeleteTenantAssignment(int id)
        {
            var bedspacesettings = await _context.TenantAssignemnts.FirstOrDefaultAsync(u => u.tenantAssignId == id);
            if (bedspacesettings == null) return false;
            bedspacesettings.isActive = false;
            //. = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpDateTenantAssignment(TenantAssignmentUpdateDTO assignemnt)
        {
            var updateassignemnts = await _context.TenantAssignemnts.FirstOrDefaultAsync(u => u.tenantAssignId == assignemnt.Assignment.tenantAssignId);
            if (updateassignemnts == null) return false;

            updateassignemnts.collectionType = assignemnt.Assignment.collectionType;
            updateassignemnts.companyID = assignemnt.Assignment.companyID;
            updateassignemnts.agreementEndDate = assignemnt.Assignment.agreementEndDate;
            updateassignemnts.agreementStartDate = assignemnt.Assignment.agreementStartDate;
            updateassignemnts.escalationPer = assignemnt.Assignment.escalationPer;
            updateassignemnts.nextescalationDate = assignemnt.Assignment.nextescalationDate;
            updateassignemnts.rentCollection = assignemnt.Assignment.rentCollection;
            updateassignemnts.propID = assignemnt.Assignment.propID;
            updateassignemnts.tenantID = assignemnt.Assignment.tenantID;
            updateassignemnts.securityAmt = assignemnt.Assignment.securityAmt;
            updateassignemnts.isActive = assignemnt.Assignment.isActive;
            updateassignemnts.notes = assignemnt.Assignment.notes;
            updateassignemnts.isClosure = assignemnt.Assignment.isClosure;
            updateassignemnts.closureDate = assignemnt.Assignment.closureDate;
            updateassignemnts.closureReason = assignemnt.Assignment.closureReason;
            updateassignemnts.refundAmount = assignemnt.Assignment.refundAmount;
            await _context.SaveChangesAsync();

            foreach (var docDto in assignemnt.Documents)
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
                        existingDoc.TenantID = docDto.TenantID;
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
                        TenantID = docDto.TenantID,
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

        public async Task<Models.TenantAssignemnt> CreateCloseAgreements(DTOs.TenantAssignment dtoAssignments)
        {

            var assignments = new Models.TenantAssignemnt()
            {
                propID = dtoAssignments.propID,
                unitID = dtoAssignments.unitID,
                tenantID = dtoAssignments.tenantID,
                companyID = dtoAssignments.companyID,
                securityAmt = dtoAssignments.securityAmt,
                rentAmt = dtoAssignments.rentAmt,
                collectionType = dtoAssignments.collectionType,
                rentCollection = dtoAssignments.rentCollection,
                agreementEndDate = dtoAssignments.agreementEndDate,
                agreementStartDate = dtoAssignments.agreementStartDate,
                escalationPer = dtoAssignments.escalationPer,
                nextescalationDate = dtoAssignments.nextescalationDate,
                isActive = dtoAssignments.isActive,
                closureDate = dtoAssignments.closureDate,
                closureReason = dtoAssignments.closureReason,
                isClosure = dtoAssignments.isClosure,
                refundAmount = dtoAssignments.refundAmount,
                notes = dtoAssignments.notes
            };
            await _context.TenantAssignemnts.AddAsync(assignments);
            await _context.SaveChangesAsync();
            return assignments;

        }

        public async Task<PagedResultDto<Models.TenantAssignemnt>> GetTenantAssignmentAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.TenantAssignemnts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.TenantName.Contains(search)); // Adjust property as needed

            }

            var totalRecords = await query.CountAsync();

            var items = await query
                // Optional: add sorting
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Models.TenantAssignemnt
                {
                    TenantName = u.Tenant != null ? u.Tenant.tenantName : null,
                    isActive = u.isActive,
                    PropName = u.Properties != null ? u.Properties.propertyName : null,
                    UnitName = u.Unit != null ? u.Unit.UnitName : null,
                    agreementStartDate = u.agreementStartDate,
                    agreementEndDate = u.agreementEndDate,

                })
                .ToListAsync();

            return new PagedResultDto<Models.TenantAssignemnt>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<Models.TenantAssignemnt> AddTenantAssignmentWithDocumentsAsync(TenantAssignmentDocumentUploadDTO dto)
        {
            var tenantAssignment = new Models.TenantAssignemnt
            {
                agreementEndDate = dto.Assignment.agreementEndDate,
                agreementStartDate = dto.Assignment.agreementStartDate,
                isActive = dto.Assignment.isActive,
                propID = dto.Assignment.propID,
                unitID = dto.Assignment.unitID,
                tenantID = dto.Assignment.tenantID,
                companyID = dto.Assignment.companyID,
                securityAmt = dto.Assignment.securityAmt,
                rentAmt = dto.Assignment.rentAmt,
                collectionType = dto.Assignment.collectionType,
                rentCollection = dto.Assignment.rentCollection,
                escalationPer = dto.Assignment.escalationPer,
                nextescalationDate = dto.Assignment.nextescalationDate,
                closureReason = dto.Assignment.closureReason,
                closureDate = dto.Assignment.closureDate,
                refundAmount = dto.Assignment.refundAmount,
                notes = dto.Assignment.notes,
                isClosure = dto.Assignment.isClosure,

            };

            _context.TenantAssignemnts.Add(tenantAssignment);
            await _context.SaveChangesAsync(); // Get TenantId

            foreach (var doc in dto.Documents)
            {

                var tenantdoc = new Models.TenantDocuments
                {
                    CompanyID = doc.CompanyID,
                    DocumentsNo = doc.DocumentsNo,
                    TenantID = tenantAssignment.tenantID,
                    DocTypeId = doc.DocTypeId,
                    Docmenturl = doc.Documenturl,
                    isActive = doc.isActive

                };

                _context.TenantDocuments.Add(tenantdoc);
                await _context.SaveChangesAsync();

            }

            return tenantAssignment;
        }

        public async Task<PagedResultDto<Models.TenantAssignemnt>> GetCloseAgreementsByCompanyId(int companyId, int pageNumber, int pageSize)
        {

            var query = _context.TenantAssignemnts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(companyId.ToString()))
            {
                query = query.Where(u => u.companyID == companyId && u.isClosure == true);
            }

            var totalRecords = await query.CountAsync();

            var items = await query
                .GroupJoin(_context.Properties,
                    ta => ta.propID,
                    p => p.PropID,
                    (ta, props) => new { ta, props })
                .SelectMany(
                    tap => tap.props.DefaultIfEmpty(),
                    (tap, prop) => new { tap.ta, Property = prop }
                )
                .GroupJoin(_context.Units,
                    tap => tap.ta.unitID,
                    u => u.UnitId,
                    (tap, units) => new { tap.ta, tap.Property, units })
                .SelectMany(
                    tapu => tapu.units.DefaultIfEmpty(),
                    (tapu, unit) => new { tapu.ta, tapu.Property, Unit = unit }
                )
                .GroupJoin(_context.Tenants,
                    tapu => tapu.ta.tenantID,
                    t => t.tenantID,
                    (tapu, tenants) => new { tapu.ta, tapu.Property, tapu.Unit, tenants })
                .SelectMany(
                    full => full.tenants.DefaultIfEmpty(),
                    (full, tenant) => new { full.ta, full.Property, full.Unit, Tenant = tenant }
                )
                .OrderBy(x => x.ta.agreementStartDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new Models.TenantAssignemnt
                {
                    tenantAssignId = x.ta.tenantAssignId,
                    tenantID = x.ta.tenantID,
                    propID = x.ta.propID,
                    unitID = x.ta.unitID,
                    companyID = x.ta.companyID,
                    agreementEndDate = x.ta.agreementEndDate,
                    agreementStartDate = x.ta.agreementStartDate,
                    securityAmt = x.ta.securityAmt,
                    rentAmt = x.ta.rentAmt,
                    collectionType = x.ta.collectionType,
                    rentCollection = x.ta.rentCollection,
                    escalationPer = x.ta.escalationPer,
                    nextescalationDate = x.ta.nextescalationDate,
                    closureDate = x.ta.closureDate,
                    closureReason = x.ta.closureReason,
                    isClosure = x.ta.isClosure,
                    PropName = x.Property != null ? x.Property.propertyName : null,
                    UnitName = x.Unit != null ? x.Unit.UnitName : null,
                    TenantName = x.Tenant != null ? x.Tenant.tenantName : null,
                    isActive = x.ta.isActive,
                    notes = x.ta.notes,
                    refundAmount = x.ta.refundAmount
                })
                .ToListAsync();

            return new PagedResultDto<Models.TenantAssignemnt>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<bool> UpDateCloseAssignment(int id, Models.TenantAssignemnt assignemnt)
        {
            var updateassignemnts = await _context.TenantAssignemnts.FirstOrDefaultAsync(u => u.tenantAssignId == id);
            if (updateassignemnts == null) return false;

            updateassignemnts.collectionType = assignemnt.collectionType;
            updateassignemnts.companyID = assignemnt.companyID;
            updateassignemnts.agreementEndDate = assignemnt.agreementEndDate;
            updateassignemnts.agreementStartDate = assignemnt.agreementStartDate;
            updateassignemnts.escalationPer = assignemnt.escalationPer;
            updateassignemnts.nextescalationDate = assignemnt.nextescalationDate;
            updateassignemnts.rentCollection = assignemnt.rentCollection;
            updateassignemnts.propID = assignemnt.propID;
            updateassignemnts.tenantID = assignemnt.tenantID;
            updateassignemnts.securityAmt = assignemnt.securityAmt;
            updateassignemnts.isActive = assignemnt.isActive;
            updateassignemnts.notes = assignemnt.notes;
            updateassignemnts.isClosure = assignemnt.isClosure;
            updateassignemnts.closureDate = assignemnt.closureDate;
            updateassignemnts.closureReason = assignemnt.closureReason;
            updateassignemnts.refundAmount = assignemnt.refundAmount;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TenantAssignmentDocumentUploadDTO> GetTenantAssignemntDocumentsbyId(int documassignmentId, int docType)
        {
            var assignment = await _context.TenantAssignemnts
        .FirstOrDefaultAsync(u => u.tenantAssignId == documassignmentId);

            if (assignment == null)
            {
                return null; // Or throw a custom exception if preferred
            }

            var assignmentDto = new DTOs.TenantAssignment
            {

                tenantID = assignment.tenantID,
                propID = assignment.propID,
                unitID = assignment.unitID,
                companyID = assignment.companyID,
                agreementEndDate = assignment.agreementEndDate,
                agreementStartDate = assignment.agreementStartDate,
                securityAmt = assignment.securityAmt,
                rentAmt = assignment.rentAmt,
                collectionType = assignment.collectionType,
                rentCollection = assignment.rentCollection,
                escalationPer = assignment.escalationPer,
                nextescalationDate = assignment.nextescalationDate,
                closureDate = assignment.closureDate,
                closureReason = assignment.closureReason,
                isClosure = assignment.isClosure,
                isActive = assignment.isActive,
                notes = assignment.notes,
                refundAmount = assignment.refundAmount
            };


            // Fetch related documents using tenantID (not unitID)
            var assignmentDocs = await _context.TenantDocuments
                .Where(u => u.TenantID == assignment.tenantID && u.DocTypeId == docType)
                .Select(u => new CreateTenantDocuments
                {
                    TenantID = u.TenantID,
                    DocTypeId = u.DocTypeId,
                    DocumentsNo = u.DocumentsNo,
                    Documenturl = u.Docmenturl,
                    isActive = u.isActive,
                    CompanyID = u.CompanyID
                })
                .ToListAsync();

            // Build and return DTO
            TenantAssignmentDocumentUploadDTO dtoassignment = new TenantAssignmentDocumentUploadDTO();
            dtoassignment.Assignment = assignmentDto;
            dtoassignment.Documents = assignmentDocs;

            return dtoassignment;
        }
    }



} 

