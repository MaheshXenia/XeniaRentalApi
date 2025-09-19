using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_NotificationSettings")]
    public class TblNotification
    {
        [Key]
        public int NotificationId { get; set; }

        public int? TemplateId { get; set; }

        public required string NotificationName { get; set; }

        public required string Description { get; set; }

        public int? CompanyId { get; set; }

        public bool? IsSMSEnabled { get; set; }

        public bool? IsEmailEnabled { get; set; }

        public bool? IsPushEnabled { get; set; }

        public required string SMSTemplate { get; set; }

        public required string EmailTemplate { get; set; }

        public string? PushTemplate { get; set; }

        public string? SMSTemplateId { get; set; }
    }
}
