using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("UpazilaCityCorporations", Schema = "com")]
    public class UpazilaCityCorporation
    {
        [Key]
        public int UpazilaCityCorporationID { get; set; }
        public string UpazilaCityCorporationCode { get; set; }
        public string UpazilaCityCorporationName { get; set; }
        public string UpazilaCityCorporationNameBangla { get; set; }
        public bool IsUpazila { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}