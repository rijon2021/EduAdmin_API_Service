using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet.ApplicationCore.DTOs.Common
{
    public class QueryObject
    {
        public object RequestObj { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Designation { get; set; }
        public int AccessRight { get; set; }
        public string BPNumber { get; set; }
        public int AppliedPost { get; set; }
        public int UserID { get; set; }
        public int UnitID { get; set; }
        public string ApplicantName { get; set; }
        public int SessionID { get; set; }
        public int UserRoleID { get; set; }
        public int CountryID { get; set; }
        public int OrganizationID { get; set; }
        public int DivisionID { get; set; }
        public int DistrictID { get; set; }
        public int UpazilaCityCorporationID { get; set; }
        public int ThanaID { get; set; }
        public int UnionWardID { get; set; }
        public int VillageAreaID { get; set; }
        public int ParaID { get; set; }
        public int TerritoryID { get; set; }
        public string TerritoryName { get; set;}
    }
}
