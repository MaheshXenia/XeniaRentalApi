using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace XeniaRentalApi.Models
{
    /// <summary>
    /// Users table for storing all types of users
    /// </summary>
    [Table("XRS_Users")]

    public class Users
    {
        /// <summary>
        /// UserId
        /// Required
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// CompanyId
        /// Required
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// UserType for storing user type like admin,tenant,landonwer
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string UserName { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string Password { get; set; }

        public  string Phone { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Is Active
        /// Required
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created Date
        /// Optional
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Modified date
        /// Optional
        /// </summary>
        public DateTime? Modifieddate { get; set; }

        [NotMapped]
        public string UsetTypeName { get; set; }

        [ForeignKey("UserType")]
        [JsonIgnore]
        public UserRoles? UserRole { get; set; }




    }
}
