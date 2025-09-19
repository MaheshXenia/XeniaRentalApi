using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class TenantDocumentGetDTO
    {
        public Tenant Tenant { get; set; }
        public List<Models.TenantDocuments> Documents { get; set; }
    }
}
