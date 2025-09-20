using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class UnitChargesDTO
    {
        public Units Unit { get; set; }
        public List<XRS_UnitChargesMapping> Charges { get; set; }
    }
}
