using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_Category")]
    public class Category
    {
        [Key]
        public int CatID { get; set; }

        public int CompanyID { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

    }
}
