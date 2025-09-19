using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class TenantWithDocumentsDto
    {
        public CreateTenant Tenant { get; set; }
        public List<CreateTenantDocuments> Documents { get; set; }

    }
}
