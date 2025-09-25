﻿using System.ComponentModel.DataAnnotations;

namespace XeniaRentalApi.DTOs
{
    public class CreateMessTypes
    {
        public int CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public required string MessName { get; set; }

        public bool IsActive { get; set; }
    }
}
