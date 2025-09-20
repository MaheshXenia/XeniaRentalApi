using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_BedSpacePlan")]
    public class XRS_BedSpacePlan
    {
        [Key]
        public int bedPlanID { get; set; }
        public int companyID { get; set; }
        public string planName { get; set; }
        public bool enableTax { get; set; }
        public string calculatedays { get; set; }
        public bool enableAdjustRent { get; set; }
        public string calculateAdjustRent { get; set; }
        public bool enablePartialRent { get; set; }
        public string calculatePartialRent { get; set; }
        public bool enableMess { get; set; }

        public string messCharge { get; set; }
        public string includeMess { get; set; }


        public string messChargeDays { get; set; }

        public string consumedDays { get; set; }

        public bool isActive { get; set; }

    }
}
