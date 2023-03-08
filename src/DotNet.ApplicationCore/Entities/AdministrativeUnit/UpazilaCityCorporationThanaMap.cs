using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("UpazilaCityCorporationThanaMaps", Schema = "com")]
    public class UpazilaCityCorporationThanaMap
    {
        [Key]
        public int UpazilaCityCorporationThanaMapID { get; set; }
        public int UpazilaCityCorporationID { get; set; }
        public int ThanaID { get; set; }
        public DateTime ValidityDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
