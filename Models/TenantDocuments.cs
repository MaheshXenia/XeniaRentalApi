using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("XRS_TenantDocuments")]
    public class TenantDocuments
    {
        /// <summary>
        /// TenantDocId
        /// Required
        /// </summary>
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TenantDocId { get; set; }

        /// <summary>
        /// DocTypeId required
        /// </summary>
        public int DocTypeId { get; set; }

        /// <summary>
        /// ComapnyId
        /// required
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TenantID { get; set; }

        /// <summary>
        /// DocumentsNo
        /// optional
        /// </summary>
        public string DocumentsNo{ get; set; }
	
        /// <summary>
        /// documents url
        /// optional
        /// </summary>
        public string Docmenturl  {get;set;}

        public bool isActive { get; set; }

        [ForeignKey("TenantID")]
        [JsonIgnore]
        public virtual Tenant? Tenant { get; set; }

        [ForeignKey("DocTypeId")]
        [JsonIgnore]
        public virtual Documents? Documents { get; set; }

        [NotMapped]
        public string? DocumentName { get; set; }

        [NotMapped]
        public bool? IsAlphaNumeric { get; set; }

        [NotMapped]
        public bool? IsExpiry { get; set; }

        [NotMapped]
        public bool? IsMandatory { get; set; }

    }
}
