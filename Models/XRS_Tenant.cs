using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Tenant")]
    public class XRS_Tenant
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tenantID { get; set; }

        public int unitID { get; set; }
        public int propID { get; set; }
        public int companyID { get; set; }

        public string tenantName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string emergencyContactNo { get; set; }
        public decimal concessionper { get; set; }
        public string note { get; set; }
        public string address { get; set; }
        public bool isActive { get; set; }

        [NotMapped]
        public string? PropName { get; set; }

        [NotMapped]
        public string? UnitName { get; set; }

        [ForeignKey("propID")]
        [JsonIgnore]
        public virtual XRS_Properties? Properties { get; set; }

        [ForeignKey("unitID")]
        [JsonIgnore]
        public virtual XRS_Units? Units { get; set; }

        [JsonIgnore] 
        public virtual ICollection<XRS_TenantDocuments>? TenantDocuments { get; set; }
    }
}
