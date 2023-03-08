using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.ApplicationCore.Entities;

namespace DotNet.ApplicationCore.DTOs.Common
{
    public class PermissionDTO : Permission
    {
        public bool? HasChild { get; set; }
        public List<Permission> ChildList { get; set; }
    }
}
