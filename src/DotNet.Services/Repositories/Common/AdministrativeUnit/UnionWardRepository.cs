using DotNet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;
using DotNet.ApplicationCore.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Infrastructure.Persistence.Contexts;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;
using DotNet.ApplicationCore.Entities;
using System.Diagnostics.Metrics;

namespace DotNet.Services.Repositories.Common.AdministrativeUnit
{
    public class UnionWardRepository : IUnionWardRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UnionWardRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMUnionWard>> GetAll()
        {
            var query =
                from unionWard in _context.UnionWards.ToList()
                join map in _context.ThanaUnionWardMaps.ToList() on unionWard.UnionWardID equals map.UnionWardID
                join thana in _context.Thanas.ToList() on map.ThanaID equals thana.ThanaID

                select new VMUnionWard
                {
                    UnionWardID = unionWard.UnionWardID,
                    UnionWardCode = unionWard.UnionWardCode,
                    UnionWardName = unionWard.UnionWardName,
                    UnionWardNameBangla = unionWard.UnionWardNameBangla,
                    GeoFenceID = 0,
                    ThanaUnionWardMap = map,
                    ThanaName = thana.ThanaName,
                    IsUnion = unionWard.IsUnion,
                };
            var retDataList = query.OrderBy(x => x.ThanaName).ThenBy(x => x.UnionWardName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMUnionWard> GetByID(int id)
        {
            var result = _context.UnionWards.SingleOrDefault(x => x.UnionWardID == id);
            var resultMap = _context.ThanaUnionWardMaps.SingleOrDefault(x => x.UnionWardID == id);

            VMUnionWard retObj = new VMUnionWard();
            retObj.UnionWardID = result.UnionWardID;
            retObj.UnionWardName = result.UnionWardName;
            retObj.UnionWardCode = result.UnionWardCode;
            retObj.UnionWardNameBangla = result.UnionWardNameBangla;
            retObj.GeoFenceID = result.GeoFenceID;
            retObj.ThanaUnionWardMap = resultMap;

            return await Task.FromResult(retObj);
        }
        public async Task<VMUnionWard> Add(VMUnionWard unionWard)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.UnionWards.SingleOrDefault(x => x.UnionWardCode == unionWard.UnionWardCode);
            if (existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            UnionWard saveUnionWard = new UnionWard();
            saveUnionWard.UnionWardName = unionWard.UnionWardName;
            saveUnionWard.UnionWardCode = unionWard.UnionWardCode;
            saveUnionWard.UnionWardNameBangla = unionWard.UnionWardNameBangla;
            saveUnionWard.GeoFenceID = unionWard.GeoFenceID;
            saveUnionWard.IsUnion = unionWard.IsUnion;
            saveUnionWard.CreatedBy = Convert.ToInt32(userID);
            saveUnionWard.CreatedDate = DateTime.Now;
            saveUnionWard.UpdatedBy = Convert.ToInt32(userID);
            saveUnionWard.UpdatedDate = DateTime.Now;
            _context.UnionWards.Add(saveUnionWard);
            _context.SaveChanges();

            ThanaUnionWardMap saveThanaUnionWardMap = new ThanaUnionWardMap();
            saveThanaUnionWardMap.ThanaID = unionWard.ThanaUnionWardMap.ThanaID;
            saveThanaUnionWardMap.UnionWardID = saveUnionWard.UnionWardID;
            saveThanaUnionWardMap.ValidityDate = DateTime.Now.AddYears(1);
            saveThanaUnionWardMap.IsActive = unionWard.ThanaUnionWardMap.IsActive;
            saveThanaUnionWardMap.CreatedBy = Convert.ToInt32(userID);
            saveThanaUnionWardMap.CreatedDate = DateTime.Now;
            saveThanaUnionWardMap.UpdatedBy = Convert.ToInt32(userID);
            saveThanaUnionWardMap.UpdatedDate = DateTime.Now;
            _context.ThanaUnionWardMaps.Add(saveThanaUnionWardMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(saveUnionWard.UnionWardID);
        }
        public async Task<VMUnionWard> Update(VMUnionWard unionWard)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            var updateUnionWard = _context.UnionWards.SingleOrDefault(x => x.UnionWardID == unionWard.UnionWardID);
            if (updateUnionWard == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.UnionWards.SingleOrDefault(x => x.UnionWardCode == unionWard.UnionWardCode && x.UnionWardID != unionWard.UnionWardID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }
            _context.Database.BeginTransaction();
            updateUnionWard.UnionWardName = unionWard.UnionWardName;
            updateUnionWard.UnionWardCode = unionWard.UnionWardCode;
            updateUnionWard.UnionWardNameBangla = unionWard.UnionWardNameBangla;
            updateUnionWard.GeoFenceID = unionWard.GeoFenceID;
            updateUnionWard.IsUnion = unionWard.IsUnion;
            updateUnionWard.UpdatedBy = userID;
            updateUnionWard.UpdatedDate = DateTime.Now;
            _context.UnionWards.Attach(updateUnionWard);
            _context.Entry(updateUnionWard).State = EntityState.Modified;

            var updateThanaUnionWardMap = _context.ThanaUnionWardMaps.SingleOrDefault(x => x.UnionWardID == unionWard.UnionWardID);
            if (updateThanaUnionWardMap != null)
            {
                updateThanaUnionWardMap.ThanaID = unionWard.ThanaUnionWardMap.ThanaID;
                updateThanaUnionWardMap.UnionWardID = updateUnionWard.UnionWardID;
                updateThanaUnionWardMap.ValidityDate = unionWard.ThanaUnionWardMap.ValidityDate;
                updateThanaUnionWardMap.IsActive = unionWard.ThanaUnionWardMap.IsActive;
                updateThanaUnionWardMap.UpdatedBy = Convert.ToInt32(userID);
                updateThanaUnionWardMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateThanaUnionWardMap);
                _context.Entry(updateThanaUnionWardMap).State = EntityState.Modified;
            }
            else
            {
                ThanaUnionWardMap saveThanaUnionWardMap = new ThanaUnionWardMap();
                saveThanaUnionWardMap.ThanaID = unionWard.ThanaUnionWardMap.ThanaID;
                saveThanaUnionWardMap.UnionWardID = updateUnionWard.UnionWardID;
                saveThanaUnionWardMap.ValidityDate = DateTime.Now.AddYears(1);
                saveThanaUnionWardMap.IsActive = unionWard.ThanaUnionWardMap.IsActive;
                saveThanaUnionWardMap.CreatedBy = Convert.ToInt32(userID);
                saveThanaUnionWardMap.CreatedDate = DateTime.Now;
                saveThanaUnionWardMap.UpdatedBy = Convert.ToInt32(userID);
                saveThanaUnionWardMap.UpdatedDate = DateTime.Now;
                _context.ThanaUnionWardMaps.Add(saveThanaUnionWardMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updateUnionWard.UnionWardID);
        }
        public async Task<bool> Delete(int UnionWardID)
        {
            var data = _context.UnionWards.SingleOrDefault(x => x.UnionWardID == UnionWardID);
            if (data != null)
            {
                var map = _context.ThanaUnionWardMaps.SingleOrDefault(map => map.UnionWardID == UnionWardID);
                if (map != null)
                {
                    _context.Entry(map).State = EntityState.Deleted;
                }
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<VMUnionWard>> UpdateOrder(List<VMUnionWard> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMUnionWard UnionWard in oList)
            {
                var map = _context.ThanaUnionWardMaps.SingleOrDefault(x => x.ThanaUnionWardMapID == UnionWard.ThanaUnionWardMap.ThanaUnionWardMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.ThanaUnionWardMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMUnionWard>> Search(QueryObject queryObject)
        {
            var mapList = _context.ThanaUnionWardMaps.ToList();
            if (queryObject.ThanaID > 0)
            {
                mapList = mapList.Where(x => x.ThanaID == queryObject.ThanaID).ToList();
            }

            var query =
                from map in mapList
                join UnionWard in _context.UnionWards on map.UnionWardID equals UnionWard.UnionWardID
                join Thana in _context.Thanas on map.ThanaID equals Thana.ThanaID

                select new VMUnionWard
                {
                    UnionWardID = UnionWard.UnionWardID,
                    UnionWardCode = UnionWard.UnionWardCode,
                    UnionWardName = UnionWard.UnionWardName,
                    UnionWardNameBangla = UnionWard.UnionWardNameBangla,
                    GeoFenceID = 0,
                    ThanaUnionWardMap = map,
                    ThanaName = Thana.ThanaName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMUnionWard>> GetListByThana(List<VMThana> objList)
        {
            return new List<VMUnionWard>();
            //int selectedOrganizationID = objList[0].OrganizationThanaMap.OrganizationID;
            //IEnumerable<string> sThanaIDs = objList.Select(x => x.ThanaID.ToString()).ToList();
            //var lstMap = _context.ThanaUnionWardMaps.Where(x => sThanaIDs.Contains(x.ThanaID.ToString())).GroupBy(x => x.UnionWardID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.ThanaUnionWardMaps.Where(x => sThanaIDs.Contains(x.ThanaID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].ThanaID == lstMapOrganization[j].ThanaID && lstMap[i].UnionWardID == lstMapOrganization[j].UnionWardID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from UnionWard in _context.UnionWards.ToList()
            //join map in lstMap on UnionWard.UnionWardID equals map.UnionWardID
            //join Thana in objList on map.ThanaID equals Thana.ThanaID

            //select new VMUnionWard
            //{
            //    UnionWardID = UnionWard.UnionWardID,
            //    UnionWardCode = UnionWard.UnionWardCode,
            //    UnionWardName = UnionWard.UnionWardName,
            //    UnionWardNameBangla = UnionWard.UnionWardNameBangla,
            //    GeoFenceID = 0,
            //    ThanaUnionWardMap = map,
            //    ThanaName = Thana.ThanaName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMUnionWard>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sThanaIDs = _context.OrganizationThanaMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.ThanaID.ToString()).ToList();
            var lstMap = _context.ThanaUnionWardMaps.Where(x => sThanaIDs.Contains(x.ThanaID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationUnionWardMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join union in _context.UnionWards on map.UnionWardID equals union.UnionWardID
                join thana in _context.Thanas on map.ThanaID equals thana.ThanaID

                join mapOrganization in lstMapOrganization on map.UnionWardID equals mapOrganization.UnionWardID into table
                from temp in table.DefaultIfEmpty()
                select new VMUnionWard
                {
                    UnionWardID = union.UnionWardID,
                    UnionWardCode = union.UnionWardCode,
                    UnionWardName = union.UnionWardName,
                    UnionWardNameBangla = union.UnionWardNameBangla,
                    GeoFenceID = union.GeoFenceID,
                    ThanaName = thana.ThanaName,
                    ThanaUnionWardMap = map,
                    OrganizationUnionWardMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x => x.UnionWardName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<bool> SaveOrganizationUnionWardMap(List<OrganizationUnionWardMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            IEnumerable<string> sOrganizationIDs = oList.Select(x => x.OrganizationID.ToString()).ToList();
            IEnumerable<string> sUnionIDs = oList.Select(x => x.UnionWardID.ToString()).ToList();
            var dbMapList = _context.OrganizationUnionWardMaps.Where(x => sOrganizationIDs.Contains(x.OrganizationID.ToString()) && sUnionIDs.Contains(x.UnionWardID.ToString())).ToList();

            for (int i = 0; i < oList.Count; i++)
            {
                var dbData = dbMapList.SingleOrDefault(x => x.OrganizationID == oList[i].OrganizationID && x.UnionWardID == oList[i].UnionWardID);
                if (dbData == null)
                {
                    if (oList[i].IsActive == true)
                    {
                        OrganizationUnionWardMap obj = new OrganizationUnionWardMap();
                        obj.OrganizationID = oList[i].OrganizationID;
                        obj.UnionWardID = oList[i].UnionWardID;
                        obj.IsActive = true;
                        obj.OrderNo = 0;
                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;
                        _context.OrganizationUnionWardMaps.Add(obj);
                    }
                }
                else
                {
                    dbData.IsActive = oList[i].IsActive;
                    dbData.OrderNo = oList[i].OrderNo;
                    dbData.UpdatedBy = userId;
                    dbData.UpdatedDate = DateTime.Now;
                    _context.OrganizationUnionWardMaps.Attach(dbData);
                    _context.Entry(dbData).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return true;

        }
    }
}

