using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Units")]
    public class Units
    {
        /// <summary>
        /// UnitId
        /// Required
        /// </summary>
        [Key]
        public int UnitId { get; set; }

        /// <summary>
        /// CompanyId
        /// Required
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// PropertyId
        /// Required
        /// </summary>
        public int PropID { get; set; }

        /// <summary>
        /// UnitName
        /// Required
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string UnitName { get; set; }

        /// <summary>
        /// Propname
        /// Required
        /// </summary>

        [NotMapped]
        public string? PropName { get; set; }

        /// <summary>
        /// Unit Type
        /// optional
        /// </summary>
        public string UnitType { get; set; }

        /// <summary>
        /// Is Active
        /// Required
        /// </summary>
        public bool IsActive { get; set; }

        public string FloorNo {  get; set; }

        public string Area {  get; set; }


        public string Remarks {  get; set; }

        public decimal DefaultRent {  get; set; }

        public decimal escalationper {  get; set; }

        public int? CatID { get; set; }

        [NotMapped]
        public string? CategoryName { get; set; }


        [ForeignKey("PropID")]
        [JsonIgnore]
        public virtual Properties? Properties { get; set; }

        [ForeignKey("CatID")]
        [JsonIgnore]
        public virtual Category? Category { get; set; }

        public ICollection<UnitChargesMapping> UnitCharges { get; set; }
    }
}
