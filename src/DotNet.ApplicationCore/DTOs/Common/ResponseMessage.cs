using DotNet.ApplicationCore.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.DTOs
{
    public class ResponseMessage
    {
        public ReturnStatus StatusCode { get; set; }
        public string Message { get; set; }
        public object ResponseObj { get; set; }
        public string Token { get; set; }
    }
}
