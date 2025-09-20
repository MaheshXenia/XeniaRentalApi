using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class TenantAssignmentDocumentGetDTO
    {
        public DTOs.TenantAssignmentDTO Tenant { get; set; }
        public List<Models.TenantDocuments> Documents { get; set; }
    }
}
