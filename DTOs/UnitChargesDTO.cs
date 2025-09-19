using XeniaRentalApi.Models;

namespace XeniaRentalApi.DTOs
{
    public class UnitChargesDTO
    {
        public Units Unit { get; set; }
        public List<UnitChargesMapping> Charges { get; set; }
    }
}
