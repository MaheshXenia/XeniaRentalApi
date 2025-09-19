namespace XeniaRentalApi.DTOs
{
    public class TenantAssignment
    {
        public int propID { get; set; }

        /// <summary>
        /// UnitId
        /// required
        /// </summary>
        public int unitID { get; set; }

        /// <summary
        /// tenantId
        /// Required
        /// </summary>

        public int tenantID { get; set; }

        /// <summary>
        /// ComapnyId
        /// Required
        /// </summary>
        public int companyID { get; set; }

        /// <summary>
        /// securityAmt
        /// optional
        /// </summary>
        public decimal securityAmt { get; set; }

        /// <summary>
        /// rentamt
        /// Optional
        /// </summary>
        public decimal rentAmt { get; set; }

        /// <summary>
        /// collection type
        /// Optional
        /// </summary>
        public string collectionType { get; set; }

        /// <summary>
        /// agreement start date
        /// </summary>
        public DateTime agreementStartDate { get; set; }

        /// <summary>
        /// agreement End date        
        /// </summary>
        public DateTime agreementEndDate { get; set; }

        /// <summary>
        /// rent Collection
        /// </summary>
        public string rentCollection { get; set; }

        /// <summary>
        /// escalationper
        /// </summary>
        public decimal escalationPer { get; set; }

        /// <summary>
        /// nextescalationdate
        /// </summary>
        public DateTime nextescalationDate { get; set; }

        /// <summary>
        /// isactive
        /// </summary>
        public bool isActive { get; set; }

        public string closureReason { get; set; }

        public DateTime closureDate { get; set; }

        public string refundAmount { get; set; }

        public string notes { get; set; }

        public bool isClosure { get; set; }
    }
}
