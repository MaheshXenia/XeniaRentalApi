using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace XeniaRentalApi.Models
{
    [Table("XRS_Charges")]
    public class Charges
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int chargeID {  get; set; }

        public int PropID { get; set; }

        public int companyID { get; set; }

        public string? chargeName { get; set; }

        [NotMapped]
        public string? PropName { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal chargeAmt { get; set; }

        public bool isVariable { get; set; }

        public bool isActive {  get; set; }

        [ForeignKey("PropID")]
        //[JsonIgnore]
        public virtual XRS_Properties? Properties { get; set; }
    }
}
