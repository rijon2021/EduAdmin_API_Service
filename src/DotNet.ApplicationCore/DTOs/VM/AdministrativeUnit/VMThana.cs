using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMThana
    {
        public int ThanaID { get; set; }
        public string ThanaCode { get; set; }
        public string ThanaName { get; set; }
        public string ThanaNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public UpazilaCityCorporationThanaMap? UpazilaCityCorporationThanaMap { get; set; }
        public OrganizationThanaMap? OrganizationThanaMap { get; set; }
        public string UpazilaCityCorporationName { get; set; }
        public int OrderNo { get; set; }
        public bool IsChecked { get; set; }
    }
}
