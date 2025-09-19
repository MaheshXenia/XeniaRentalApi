﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Company")]
    public class Company
    {
        [Key]
        public int companyID {  get; set; }

        public required string companyName { get; set; }

        public string address { get; set; }

        public  string email { get; set; }

        public string phoneNumber { get; set; }

        public  string pin { get; set; }

        public string logo { get; set; }

        public bool IsActive { get; set; }



    }
}
