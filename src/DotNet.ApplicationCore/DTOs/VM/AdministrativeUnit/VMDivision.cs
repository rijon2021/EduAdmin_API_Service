using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMDivision
    {
        public int DivisionID { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DivisionNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public CountryDivisionMap? CountryDivisionMap { get; set; }
        public OrganizationDivisionMap? OrganizationDivisionMap { get; set; }
        public string CountryName { get; set; }
        public int? OrderNo { get; set; }
        public bool? IsChecked { get; set; }
    }
}
