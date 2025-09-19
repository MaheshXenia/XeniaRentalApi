namespace XeniaRentalApi.DTOs
{
    public class BedSpaceAssignment
    {
        public int propID { get; set; }

        public int unitID { get; set; }

        public int tenantID { get; set; }

        public int bedSpaceID { get; set; }

        public int companyID { get; set; }

        public decimal rentAmt { get; set; }

        public bool isTaxable { get; set; }

        public string taxRatePer { get; set; }

        public string rentConcession { get; set; }

        public string messConcession { get; set; }

        public DateTime agreementStartDate { get; set; }

        public DateTime agreementEndDate { get; set; }

        public string frequency { get; set; }

        public DateTime rentDueDate { get; set; }

        public decimal rentescalationPer { get; set; }

        public DateTime escalationDate { get; set; }

        public decimal securityAmt { get; set; }

        public decimal charges { get; set; }

        public decimal amount { get; set; }

        public bool isActive { get; set; }

    }
}
