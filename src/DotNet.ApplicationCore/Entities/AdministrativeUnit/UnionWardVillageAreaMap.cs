using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("UnionWardVillageAreaMaps", Schema = "com")]
    public class UnionWardVillageAreaMap
    {
        [Key]
        public int UnionWardVillageAreaMapID { get; set; }
        public int UnionWardID { get; set; }
        public int VillageAreaID { get; set; }
        public DateTime ValidityDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
