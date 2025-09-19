namespace XeniaRentalApi.DTOs
{
    public class CreateDocuments
    {
        public int companyID { get; set; }

        public int propID { get; set; }

        public string docName { get; set; }

        public string docPurpose { get; set; }

        public bool isAlphanumeric { get; set; }

        public bool isExpiry { get; set; }

        public bool isMandatory { get; set; }

        public bool isActive { get; set; }
    }
}
