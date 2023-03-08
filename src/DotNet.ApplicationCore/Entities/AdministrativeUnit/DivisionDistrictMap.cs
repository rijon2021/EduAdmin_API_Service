using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("DivisionDistrictMaps", Schema = "com")]
    public class DivisionDistrictMap
    {
        [Key]
        public int DivisionDistrictMapID { get; set; }
        public int DivisionID { get; set; }
        public int DistrictID { get; set; }
        public DateTime ValidityDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
