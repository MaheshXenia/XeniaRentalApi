using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    [Table("XRS_UnitChargesMapping")]
    public class UnitChargesMapping
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int unitMapID { get; set; }

        public int chargeID { get; set; }

        public int unitID {  get; set; }

        public int propID { get; set; }

        public int companyID { get; set; }

        public string frequency { get; set; }

        public decimal amount { get; set; }

        public bool isActive { get; set; }

        [NotMapped]
        public string? ChargeName { get; set; }

        [NotMapped]
        public string? ChargeType { get; set; }


        [ForeignKey("chargeID")]
        [JsonIgnore]
        public virtual Charges? Charges { get; set; }


    }
}
