using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMUpazilaCityCorporation
    {
        public int UpazilaCityCorporationID { get; set; }
        public string UpazilaCityCorporationCode { get; set; }
        public string UpazilaCityCorporationName { get; set; }
        public string UpazilaCityCorporationNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public DistrictUpazilaCityCorporationMap? DistrictUpazilaCityCorporationMap { get; set; }
        public OrganizationUpazilaCityCorporationMap? OrganizationUpazilaCityCorporationMap { get; set; }
        public string DistrictName { get; set; }
        public int OrderNo { get; set; }
        public bool IsUpazila { get; set; }
        public bool? IsChecked { get; set; }
    }
}
