using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    /// <summary>
    /// Voucher class
    /// </summary>
    [Table("XRS_Voucher")]
    public class Voucher
    {
        /// <summary>
        /// VoucherId
        /// Required
        /// </summary>
        [Key]
        public int VoucherID { get; set; }

        public int unitID {  get; set; }


        /// <summary>
        /// Company Id
        /// required
        /// </summary>
        public int  CompanyID { get; set; }

        
        /// <summary>
        /// Property Id
        /// required
        /// </summary>
        public int PropID { get; set; }

        /// <summary>
        /// VoucherId
        /// required
        /// </summary>
        public string? VoucherNo { get; set; }

        /// <summary>
        /// VoucherDate
        /// optional
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
        /// VoucherType
        /// optional
        /// </summary>
        public string? VoucherType { get; set; }

        /// <summary>
        /// DrID
        /// optional
        /// </summary>
        public string? DrID { get; set; }

        /// <summary>
        /// CrId
        /// optional
        /// </summary>
        public string? CrID { get; set; }

        /// <summary>
        /// Amount
        /// optional
        /// </summary>
        public string? Amount { get; set; }

        /// <summary>
        /// RefNo
        /// optional
        /// </summary>
        public string? RefNo { get; set; }

        /// <summary>
        /// Remarks
        /// optional
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Issueing Bank
        /// optional
        /// </summary>
        public string? IssueingBank { get; set; }

        /// <summary>
        /// ChequeNo
        /// optional
        /// </summary>
        public string? ChequeNo { get; set; }

        /// <summary>
        /// Cancelled
        /// optional
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// CrAmount
        /// required
        /// </summary>
        public decimal CrAmount { get; set; }

        /// <summary>
        /// Is Reconcil
        /// optional
        /// </summary>
        public bool IsReconcil { get; set; }

        /// <summary>
        /// ChequeStatus
        /// optional
        /// </summary>
        public bool? ChequeStatus { get; set; }

        /// <summary>
        /// Reconcildate
        /// optional
        /// </summary>
        public DateTime? ReconcilDate { get; set; }

        /// <summary>
        /// Created on date
        /// optional
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Created By
        /// optional
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Modified on date
        /// optional
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Modified By
        /// Optional
        /// </summary>
        public DateTime? ModificationBy { get; set; }

        public int paidByUser { get; set; }

        public bool isActive {  get; set; }

    }
}
