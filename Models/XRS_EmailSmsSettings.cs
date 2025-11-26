using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_EmailSmsSettings")]
    public class XRS_EmailSmsSettings
    {
        [Key]
        public int Id { get; set; }

        public int companyID { get; set; }

        public string? emailIincomingServer { get; set; }

        public string? emailSender { get; set; }

        public string? password { get; set; }

        public bool userDefaultCredentials { get; set; }

        public string? port { get; set; }

        public bool? enableSsl { get; set; }

        public string? host { get; set; }
        public string? smsGateWay { get; set; }

        public bool active { get; set; }
    }
}
