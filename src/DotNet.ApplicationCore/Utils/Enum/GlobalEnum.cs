using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.Utils.Enum
{
    public class GlobalEnum
    {
        public enum DataSchema
        {
            dbo = 1,
            core = 2,
            com = 3
        }
        public enum ReturnStatus
        {
            Success = 1,
            Failed = -1,
            Duplicate = 2,
            PendingOTPAuthentication = 3,
        }
        public enum PermissionType
        {
            Menu = 1,
            Button = 2,
            Role = 3,
        }
        public enum NotificationType
        {
            All = 0,
            SMS = 1,
            Email = 2,
            Push = 3,
        }
        public enum GeoFenceType
        {
            None = 0,
            All = 1
        }
        public enum GlobalSettingsEnum
        {
            Login_Session_Time = 1,
            SMS_Base_Url = 2,
            Google_Map_Key = 3
        }
        public enum NotificationAreaEnum
        {
            UserRegistration = 1,
            UserLogin = 2,
        }
        public enum OrganizationType
        {
            Govt = 1,
            Private = 2,
        }
    }
}
