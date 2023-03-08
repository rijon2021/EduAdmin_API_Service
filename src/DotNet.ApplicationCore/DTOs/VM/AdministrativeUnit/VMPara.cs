using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMPara
    {
        public int ParaID { get; set; }
        public string ParaCode { get; set; }
        public string ParaName { get; set; }
        public string ParaNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public VillageAreaParaMap? VillageAreaParaMap { get; set; }
        public OrganizationParaMap? OrganizationParaMap { get; set; }
        public string VillageAreaName { get; set; }
        public int OrderNo { get; set; }
        public bool IsChecked { get; set; }
    }
}
