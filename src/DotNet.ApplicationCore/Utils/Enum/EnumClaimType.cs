using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet.ApplicationCore.Utils.Enum
{
    public enum EnumClaimType
    {
        UserID,
        UserFullName,
        CompanyName,
        OrganizationID,
        Email,
        Rights,
        TimeZoneID,
        SessionID,
        DateTimeFormat,
        CompanyGuid,
        RoleID,
        LanguageCountryID,
        BranchID,
        BranchGuid,
        UserTypeID,
        IsRedisCacheEnable,
        UserAutoID
    }
    public class ClaimTypeObj
    {
        public string UserID { get; set; }
        public string UserFullName { get; set; }
        public string CompanyName { get; set; }
        public int OrganizationID { get; set; }
        public string Email { get; set; }
        public string Rights { get; set; }
        public string TimeZoneID { get; set; }
        public string SessionID { get; set; }
        public string DateTimeFormat { get; set; }
        public string CompanyGuid { get; set; }
        public string RoleID { get; set; }
        public string LanguageCountryID{ get; set; }
        public string BranchID { get; set; }
        public string BranchGuid { get; set; }
        public string UserTypeID { get; set; }
        public string IsRedisCacheEnable { get; set; }
        public string UserAutoID { get; set; }

    }
}
