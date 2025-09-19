using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Documents")]
    public class Documents
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int docTypeId { get; set; }

        public int companyID { get; set; }

        public int propID { get; set; }

        public string  docName { get; set; }

        public string docPurpose { get; set; }

        public bool isAlphanumeric { get; set; }

        public bool isExpiry { get; set; }

        public bool isMandatory { get; set; }

        public bool isActive { get; set; }

        [NotMapped]
        public string? propName {  get; set; }

        [ForeignKey("propID")]
        [JsonIgnore]
        public virtual Properties? Properties { get; set; }
    }

}
