using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMCountry
    {
        public int CountryID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public OrganizationCountryMap OrganizationCountryMap { get; set; }
        public string OrganizationName { get; set; }
        public bool IsChecked { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int UpdatedBy { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
