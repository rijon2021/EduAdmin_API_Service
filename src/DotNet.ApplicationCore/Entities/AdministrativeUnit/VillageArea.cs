using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("VillageAreas", Schema = "com")]
    public class VillageArea
    {
        [Key]
        public int VillageAreaID { get; set; }
        public string VillageAreaCode { get; set; }
        public string VillageAreaName { get; set; }
        public string VillageAreaNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}