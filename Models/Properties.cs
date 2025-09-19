using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Properties")]
    public class Properties
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropID { get; set; }

        /// <summary>
        /// CompanyId
        /// Required
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// UserType for storing user type like admin,tenant,landonwer
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string propertyName { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        
        public string propertyType { get; set; }

       

        /// <summary>
        /// Is Active
        /// Required
        /// </summary>
        public bool IsActive { get; set; }

    
    }
}
