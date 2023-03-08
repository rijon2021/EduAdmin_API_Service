using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("UnionWards", Schema = "com")]
    public class UnionWard
    {
        [Key]
        public int UnionWardID { get; set; }
        public string UnionWardCode { get; set; }
        public string UnionWardName { get; set; }
        public string UnionWardNameBangla { get; set; }
        public bool IsUnion { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}