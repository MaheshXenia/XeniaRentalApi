using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_MessDetails")]
    public class MessDetails
    {
        [Key]
        public int messdtsID { get; set; }

        /// <summary>
        /// CompanyId
        /// Required
        /// </summary>
        public int messID { get; set; }

        /// <summary>
        /// UserType for storing user type like admin,tenant,landonwer
        /// </summary>
        
        public int userID { get; set; }

        public int propID { get; set; }

        public int compantID { get; set; }

        public int tenantID { get; set; }
        
        public DateTime messDate { get; set; }
        public bool isConsumed { get; set; }

        [NotMapped]
        public string? PropName { get; set; }

        [NotMapped]
        public string? TenantName { get; set; }

        [NotMapped]
        public string? MessTypeName { get; set; }

        [ForeignKey("propID")]
        [JsonIgnore]
        public virtual Properties? Properties { get; set; }

        [ForeignKey("tenantID")]
        [JsonIgnore]
        public virtual Tenant? Tenant { get; set; }

        [ForeignKey("messID")]
        [JsonIgnore]
        public virtual MessTypes? MessTypes { get; set; }

    }
}
