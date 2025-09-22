using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaRentalApi.Models
{
    [Table("XRS_EmailSmsSettings")]
    public class XRS_EmailSmsSettings
    {
        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string? EmailIncomingServer { get; set; }

        public string? EmailSender { get; set; }

        public string? Password { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public string? Port { get; set; }

        public bool? EnableSsl { get; set; }

        public string? Host { get; set; }
        public string? SMSGateway { get; set; }

        public bool Active { get; set; }
    }
}
