using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Tenant")]
    public class Tenant
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tenantID { get; set; }

        /// <summary>
        /// CompanyId
        /// Required
        /// </summary>
        public int unitID { get; set; }

        /// <summary>
        /// UnitId
        /// required
        /// </summary>
        public int propID { get; set; }

        

        
        /// <summary>
        /// ComapnyId
        /// Required
        /// </summary>
        public int companyID { get; set; }

        /// <summary>
        /// securityAmt
        /// optional
        /// </summary>
        public string tenantName { get; set; }

        /// <summary>
        /// rentamt
        /// Optional
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// collection type
        /// Optional
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// agreement start date
        /// </summary>
        public string emergencyContactNo { get; set; }

        /// <summary>
        /// agreement End date        
        /// </summary>
        public decimal  concessionper { get; set; }

        /// <summary>
        /// rent Collection
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// escalationper
        /// </summary>
        public string  address { get; set; }

      
        /// <summary>
        /// isactive
        /// </summary>
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


    }
}
