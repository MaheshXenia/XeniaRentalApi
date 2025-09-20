namespace XeniaRentalApi.Dtos
{
    public class TenantAssignmentCreateDto
    {
        public int propID { get; set; }
        public int unitID { get; set; }
        public int tenantID { get; set; }
        public int companyID { get; set; }
        public decimal securityAmt { get; set; }
        public bool isTaxable { get; set; }
        public int bedSpaceID { get; set; }
        public decimal rentAmt { get; set; }
        public decimal rentConcession { get; set; }
        public decimal messConcession { get; set; }
        public string frequency { get; set; }
        public string collectionType { get; set; }
        public DateTime agreementStartDate { get; set; }
        public DateTime agreementEndDate { get; set; }
        public string rentCollection { get; set; }
        public decimal escalationPer { get; set; }
        public DateTime? nextescalationDate { get; set; }
        public DateTime? rentDueDate { get; set; }
        public string? notes { get; set; }
        public List<TenantCreateDocumentDto>? Documents { get; set; }
    }
}
