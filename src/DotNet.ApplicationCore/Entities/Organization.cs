using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.ApplicationCore.Entities
{
    [Table("Organizations", Schema = "com")]
    public class Organization
    {
        [Key]
        public int OrganizationID { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public OrganizationType OrganizationType { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Address { get; set; }
        public string OrganizationLogoURL { get; set; }
        public string ProductLogoURL { get; set; }
        public string PublicURL { get; set; }
        public string PrivateURL { get; set; }
        public int? ContactPersonID { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? OrganizationUserID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        [NotMapped]
        public string OrganizationTypeStr 
        {
            get 
            {
                return this.OrganizationType.ToString();
            
            }
        }
    }
}