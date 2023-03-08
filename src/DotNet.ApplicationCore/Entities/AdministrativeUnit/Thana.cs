using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("Thanas", Schema = "com")]
    public class Thana
    {
        [Key]
        public int ThanaID { get; set; }
        public string ThanaCode { get; set; }
        public string ThanaName { get; set; }
        public string ThanaNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}