using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Accounts")]
    public class XRS_Accounts
    {
        [Key]
        public int accID { get; set; }

        public int companyID { get; set; }

        public int VoucherId { get; set; }

        public string invType { get; set; }

        public string invNo { get; set; }

        public DateTime invDate { get; set; }

        public string ledgerDr { get; set; }

        public string ledgerCr { get; set; }

        public decimal amountDr { get; set; }

        public decimal amountCr { get; set; }

        public string remarks { get; set; }

        public DateTime createdOn { get; set; }

        public DateTime modifiedOn { get; set; }

        public string createdBy { get; set; }

        public string modifiedBy { get; set; }

        public bool isActive { get;set; }   
    }
}
