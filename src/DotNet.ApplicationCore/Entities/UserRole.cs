using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet.ApplicationCore.Entities
{
    [Table("UserRoles", Schema = "com")]
    public class UserRole
    {
        [Key]
        public int UserRoleID { get; set; }
        public string UserRoleName { get; set; }
        public bool IsActive { get; set; }
        public int OrderNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}