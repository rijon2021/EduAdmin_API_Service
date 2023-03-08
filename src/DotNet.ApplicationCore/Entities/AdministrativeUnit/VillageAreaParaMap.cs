using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("VillageAreaParaMaps", Schema = "com")]
    public class VillageAreaParaMap
    {
        [Key]
        public int VillageAreaParaMapID { get; set; }
        public int VillageAreaID { get; set; }
        public int ParaID { get; set; }
        public DateTime ValidityDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
