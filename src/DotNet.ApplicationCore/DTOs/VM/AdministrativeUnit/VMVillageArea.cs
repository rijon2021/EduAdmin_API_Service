using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMVillageArea
    {
        public int VillageAreaID { get; set; }
        public string VillageAreaCode { get; set; }
        public string VillageAreaName { get; set; }
        public string VillageAreaNameBangla { get; set; }
        public int? GeoFenceID { get; set; }
        public UnionWardVillageAreaMap? UnionWardVillageAreaMap { get; set; }
        public OrganizationVillageAreaMap? OrganizationVillageAreaMap { get; set; }
        public string UnionWardName { get; set; }
        public int OrderNo { get; set; }
        public bool IsChecked { get; set; }
    }
}
