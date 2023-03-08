using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit
{
    public class VMUnionWard
    {
        public int UnionWardID { get; set; }
        public string UnionWardCode { get; set; }
        public string UnionWardName { get; set; }
        public string UnionWardNameBangla { get; set; }
        public bool IsUnion { get; set; }
        public int? GeoFenceID { get; set; }
        public ThanaUnionWardMap? ThanaUnionWardMap { get; set; }
        public OrganizationUnionWardMap? OrganizationUnionWardMap { get; set; }
        public string ThanaName { get; set; }
        public int OrderNo { get; set; }
        public bool IsChecked { get; set; }
    }
}
