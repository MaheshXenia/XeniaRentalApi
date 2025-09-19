namespace XeniaRentalApi.DTOs
{
    public class TenantDocumentUploadDto
    {
        public IFormFile File { get; set; }
        public int DocTypeId { get; set; }

        /// <summary>
        /// ComapnyId
        /// required
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TenantID { get; set; }

        /// <summary>
        /// DocumentsNo
        /// optional
        /// </summary>
        public string DocumentsNo { get; set; }

        /// <summary>
        /// documents url
        /// optional
        /// </summary>
        public string Documenturl { get; set; }

        public bool isActive { get; set; }

    }
}
