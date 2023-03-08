using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities
{
    [Table("NotificationAreas", Schema = "core")]
    public class NotificationArea
    {
        [Key]
        public int NotificationAreaID { get; set; }
        public string NotificationAreaName { get; set; }
        public NotificationType NotificationType { get; set; }
        public string NotificationBody { get; set; }
        public bool IsActive { get; set; }
        public int ExpireTime { get; set; } = 5;
        [NotMapped]
        public string NotificationTypeName
        {
            get
            {
                return this.NotificationType.ToString();
            }
        }
    }
}
