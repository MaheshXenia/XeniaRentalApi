using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    /// <summary>
    /// User Roles
    /// </summary>
    [Table("XRS_UserRole")]

    public class XRS_UserRole
    {
        /// <summary>
        /// User roleId
        /// Required
        /// </summary>
        [Key]

        public int UserRoleId { get; set; }

        /// <summary>
        /// CompanyId
        /// Required
        /// </summary>
        public int CompanyId { get; set; }
      
        [Required]
        [StringLength(50)]
        public required string UserRoleName { get; set; }

        /// <summary>
        /// IsActive
        /// Required
        /// </summary>
        public bool isActive { get; set; }

        /// <summary>
        /// Created Date
        /// Optional
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Modified date
        /// Optional
        /// </summary>
        public DateTime Modifieddate { get; set; }




    }
}
