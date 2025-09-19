using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Messtypes")]
    public class MessTypes
    {
        [Key]
        public int messID { get; set; }

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
        public required string MessName { get; set; }

       


        /// <summary>
        /// Is Active
        /// Required
        /// </summary>
        public bool IsActive { get; set; }
    }
}
