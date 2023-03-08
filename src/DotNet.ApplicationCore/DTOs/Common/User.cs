using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNet.ApplicationCore.DTOs
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserFullName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UpdateUserRequest : CreateUserRequest
    {
        [StringLength(30, MinimumLength = 3)]
        public int MobileNo { get; set; }
    }

    public class UserResponse
    {
        public string UserID { get; set; }
        public string UserFullName { get; set; }
        public string Password { get; set; }
        public int MobileNo { get; set; }
    }
    public class AuthUser
    {
        public int UserAutoID { get; set; }
        public string UserID { get; set; }
        public int UserTypeID { get; set; }
        public int OrganizationID { get; set; }
        public int DesignationID { get; set; }
        public string UserFullName { get; set; }
        public int UserRoleID { get; set; }
        public TokenResult TokenResult { get; set; }
        public List<PermissionDTO>? Permissions { get; set; }
        public List<GlobalSetting>? GlobalSettings { get; set; }
        public string Password { get; set; }
    }
}