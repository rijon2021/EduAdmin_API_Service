using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities
{
    [Table("Paras", Schema = "com")]
    public class Para
    {
        [Key]
        public int ParaID { get; set; }
        public string ParaCode { get; set; }
        public string ParaName { get; set; }
        public string ParaNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}