using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class TenantDocumentDTO
    {
        public Tenant Tenant { get; set; }
        public List<CreateTenantDocuments> Documents { get; set; }
    }
}
