namespace XeniaRentalApi.DTOs
{
    public class TenantAssignmentDocumentUploadDTO
    {
        public TenantAssignment Assignment { get; set; }
        public List<CreateTenantDocuments> Documents { get; set; }
    }

    public class TenantAssignmentUpdateDTO
    {
        public Models.TenantAssignemnt Assignment { get; set; }
        public List<CreateTenantDocuments> Documents { get; set; }
    }
}
