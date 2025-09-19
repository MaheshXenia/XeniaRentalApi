using System.ComponentModel.DataAnnotations;

namespace XeniaRentalApi.DTOs
{
    public class CreateMessTypes
    {
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
