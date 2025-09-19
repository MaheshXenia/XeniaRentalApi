using System.ComponentModel.DataAnnotations;

namespace XeniaRentalApi.DTOs
{
    public class CreateProperties
    {
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
