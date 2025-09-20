using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_TenantAssignment")]
    public class XRS_TenantAssignment
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tenantAssignId { get; set; }


        public int propID { get; set; }

        public int unitID { get; set; }

        public int tenantID { get; set; }

        public int companyID { get; set; }

        public decimal securityAmt { get; set; }

        public bool isTaxable { get; set; }

        public int bedSpaceID { get; set; }

        public decimal rentAmt { get; set; }

        public decimal rentConcession { get; set; }

        public decimal messConcession { get; set; }

        public string frequency { get; set; }

        public string collectionType { get; set; }

        public DateTime agreementStartDate { get; set; }

        public DateTime agreementEndDate { get; set; }

        public string rentCollection { get; set; }

        public decimal escalationPer { get; set; }

        public DateTime? nextescalationDate { get; set; }

        public DateTime? rentDueDate { get; set; }

        public string closureReason { get; set; }

        public DateTime? closureDate { get; set; }

        public decimal refundAmount { get; set; }

        public string charges { get; set; }

        public decimal amount { get; set; }

        public string notes { get; set; }

        public bool isClosure { get; set; }

        public bool isActive { get; set; }

        [NotMapped]
        public string? PropName { get; set; }

        [NotMapped]
        public string? UnitName { get; set; }

        [NotMapped]
        public string? TenantName { get; set; }

        [NotMapped]
        public string? TenantContactNo { get; set; }


        [ForeignKey("propID")]
        [JsonIgnore]
        public virtual XRS_Properties? Properties { get; set; }

        [ForeignKey("tenantID")]
        [JsonIgnore]
        public virtual XRS_Tenant? Tenant{ get; set; }

        [ForeignKey("unitID")]
        [JsonIgnore]
        public virtual XRS_Units? Unit { get; set; }
/*
        [JsonIgnore]
        public virtual ICollection<XRS_TenantDocuments>? TenantDocuments { get; set; }*/

    }
}
