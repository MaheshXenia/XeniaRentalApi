using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class TenantAssignmentDocumentGetDTO
    {
        public DTOs.TenantAssignmentDTO Tenant { get; set; }
        public List<Models.XRS_TenantDocuments> Documents { get; set; }
    }
}
