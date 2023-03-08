using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities
{
    [Table("GlobalSettings", Schema = "core")]
    public class GlobalSetting
    {
        [Key]
        public int GlobalSettingID { get; set; }
        public string GlobalSettingName { get; set; }
        public int Value { get; set; }
        public string ValueInString { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}