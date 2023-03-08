using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities
{
    [Table("PermissionUserRoleMaps", Schema = "core")]
    public class PermissionUserRoleMap
    {
        [Key]
        public int PermissionUserRoleMapID { get; set; }
        public int PermissionID { get; set; }
        public int UserRoleID { get; set; }
        public int OrganizationID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
      
    }
}
