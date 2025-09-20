﻿using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.DTOs
{
    public class CategoryDto
    {
        public int CompanyID { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

    }
}
