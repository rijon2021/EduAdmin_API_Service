using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities.AdministrativeUnit
{
    [Table("Divisions", Schema = "com")]
    public class Division
    {
        [Key]
        public int DivisionID { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DivisionNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}