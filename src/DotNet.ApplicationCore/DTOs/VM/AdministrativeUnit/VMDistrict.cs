using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMDistrict
    {
        public int DistrictID { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public DivisionDistrictMap? DivisionDistrictMap { get; set; }
        public OrganizationDistrictMap? OrganizationDistrictMap { get; set; }
        public string DivisionName { get; set; }
        public int OrderNo { get; set; }
        public bool IsChecked { get; set; }
        public int CountryID{ get; set; }
        public string CountryName { get; set; }
    }
}
