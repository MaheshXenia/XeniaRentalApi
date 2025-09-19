using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.DTOs
{
    public class CreateCategory
    {
        public int CompanyID { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

    }
}
