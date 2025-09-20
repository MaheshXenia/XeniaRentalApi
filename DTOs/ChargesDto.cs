﻿using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.DTOs
{
    public class ChargesDto
    {
        public int chargeID { get; set; }
        public string? chargeName { get; set; }
        public int PropID { get; set; }
        public int companyID { get; set; }
        public decimal chargeAmt { get; set; }
        public bool isVariable { get; set; }
        public bool isActive { get; set; }
        public string? PropName { get; set; }
    }
}
