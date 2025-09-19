using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.DTOs
{
    public class CreateCharges
    {
        public int PropID { get; set; }

        public int companyID { get; set; }

        public string chargeName { get; set; }

       
        [Column(TypeName = "decimal(18,3)")]
        public decimal chargeAmt { get; set; }

        public bool isVariable { get; set; }

        public bool isActive { get; set; }
    }
}
