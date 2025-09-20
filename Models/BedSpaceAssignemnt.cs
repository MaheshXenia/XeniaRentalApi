using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_BedspaceAssignment")]
    public class BedSpaceAssignemnt
    {
        [Key]
        public int assignmentID {  get; set; }

        public int propID { get; set; }

        public int unitID { get; set; }

        public int tenantID { get; set; }

        public int bedSpaceID { get; set; }

        public int companyID { get;set; }

        public decimal rentAmt { get; set; }

        public bool isTaxable { get; set; }

        public string taxRatePer { get; set; }

        public string rentConcession { get; set; }

        public string messConcession { get; set; }  

        public DateTime agreementStartDate { get; set; }

        public DateTime agreementEndDate { get; set; }

        public string frequency { get; set; }

        public DateTime rentDueDate { get; set; }

        public decimal rentescalationPer { get; set; }

        public DateTime escalationDate { get; set; }

        public decimal securityAmt { get; set; }

        public decimal charges { get; set; }

        public decimal amount { get; set; }

        public bool isActive { get; set; }

        [NotMapped]
        public string? PropName { get; set; }

        [NotMapped]
        public string? UnitName { get; set; }

        [NotMapped]
        public string? TenantName { get; set; }

        [NotMapped]
        public string? BedSpaceName { get; set; }

        [ForeignKey("propID")]
        [JsonIgnore]
        public virtual XRS_Properties? Properties { get; set; }

        [ForeignKey("unitID")]
        [JsonIgnore]
        public virtual XRS_Units? Units { get; set; }

        [ForeignKey("tenantID")]
        [JsonIgnore]
        public virtual Tenant? Tenant { get; set; }

        [ForeignKey("bedSpaceID")]
        [JsonIgnore]
        public virtual BedSpace? BedSpace { get; set; }

    }
}
