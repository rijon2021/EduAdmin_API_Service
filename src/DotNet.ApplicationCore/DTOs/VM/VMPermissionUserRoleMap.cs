using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.DTOs
{
    public class VMPermissionUserRoleMap
    {   
        public int? PermissionUserRoleMapID { get; set; }
        public int PermissionID { get; set; }
        public int ParentPermissionID { get; set; }
        public int UserRoleID { get; set; }
        public int? OrganizationID { get; set; }
        public string PermissionName { get; set; }
        public string DisplayName { get; set; }
        public PermissionType PermissionType { get; set; }
        public string RoutePath { get; set; }
        public bool IsChecked { get; set; }
        public string PermissionTypeStr
        {
            get { 
            return PermissionType.ToString();
            }
        }
    }
}
