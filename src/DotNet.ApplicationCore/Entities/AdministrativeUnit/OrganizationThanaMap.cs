
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("OrganizationThanaMaps", Schema = "com")]
    public class OrganizationThanaMap
    {
        [Key]
        public int OrganizationThanaMapID { get; set; }
        public int OrganizationID { get; set; }
        public int ThanaID { get; set; }
        public int OrderNo { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}