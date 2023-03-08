using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("Districts", Schema = "com")]
    public class District
    {
        [Key]
        public int DistrictID { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}