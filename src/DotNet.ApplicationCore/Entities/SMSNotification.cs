using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet.ApplicationCore.Entities
{
    [Table("SMSNotifications", Schema = "core")]
    public class SMSNotification
    {
        [Key]
        public int SMSNotificationID { get; set; }
        public int UserID { get; set; }
        public int NotificationAreaID { get; set; }
        public string RecipientMobileNo { get; set; }
        public string OTP { get; set; }
        public string MessageBody { get; set; }
        public bool SendingStatus { get; set; } = false;
        public bool DeliveryStatus { get; set; } = false;
        public string ReponseMessage { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }
}