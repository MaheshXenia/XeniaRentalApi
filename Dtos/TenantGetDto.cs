using XeniaRentalApi.Dtos;
using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class TenantGetDto
    {
        public XRS_Tenant Tenant { get; set; }
        public List<TenantDocumentDto> Documents { get; set; }
    }
}
