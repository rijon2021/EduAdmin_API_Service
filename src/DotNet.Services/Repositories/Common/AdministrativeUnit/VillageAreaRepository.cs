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
    public class VillageAreaRepository : IVillageAreaRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public VillageAreaRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMVillageArea>> GetAll()
        {
            var query =
                from villageArea in _context.VillageAreas.ToList()
                join map in _context.UnionWardVillageAreaMaps.ToList() on villageArea.VillageAreaID equals map.VillageAreaID
                join unionWard in _context.UnionWards.ToList() on map.UnionWardID equals unionWard.UnionWardID

                select new VMVillageArea
                {
                    VillageAreaID = villageArea.VillageAreaID,
                    VillageAreaCode = villageArea.VillageAreaCode,
                    VillageAreaName = villageArea.VillageAreaName,
                    VillageAreaNameBangla = villageArea.VillageAreaNameBangla,
                    GeoFenceID = 0,
                    UnionWardVillageAreaMap = map,
                    UnionWardName = unionWard.UnionWardName,
                };
            var retDataList = query.OrderBy(x => x.UnionWardName).ThenBy(x => x.VillageAreaName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMVillageArea> GetByID(int id)
        {
            var result = _context.VillageAreas.SingleOrDefault(x => x.VillageAreaID == id);
            var resultMap = _context.UnionWardVillageAreaMaps.SingleOrDefault(x => x.VillageAreaID == id);

            VMVillageArea retObj = new VMVillageArea();
            retObj.VillageAreaID = result.VillageAreaID;
            retObj.VillageAreaName = result.VillageAreaName;
            retObj.VillageAreaCode = result.VillageAreaCode;
            retObj.VillageAreaNameBangla = result.VillageAreaNameBangla;
            retObj.GeoFenceID = result.GeoFenceID;
            retObj.UnionWardVillageAreaMap = resultMap;

            return await Task.FromResult(retObj);
        }
        public async Task<VMVillageArea> Add(VMVillageArea villageArea)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.VillageAreas.SingleOrDefault(x => x.VillageAreaCode == villageArea.VillageAreaCode);
            if (existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            VillageArea saveVillageArea = new VillageArea();
            saveVillageArea.VillageAreaName = villageArea.VillageAreaName;
            saveVillageArea.VillageAreaCode = villageArea.VillageAreaCode;
            saveVillageArea.VillageAreaNameBangla = villageArea.VillageAreaNameBangla;
            saveVillageArea.GeoFenceID = villageArea.GeoFenceID;
            saveVillageArea.CreatedBy = Convert.ToInt32(userID);
            saveVillageArea.CreatedDate = DateTime.Now;
            saveVillageArea.UpdatedBy = Convert.ToInt32(userID);
            saveVillageArea.UpdatedDate = DateTime.Now;
            _context.VillageAreas.Add(saveVillageArea);
            _context.SaveChanges();

            UnionWardVillageAreaMap saveUnionWardVillageAreaMap = new UnionWardVillageAreaMap();
            saveUnionWardVillageAreaMap.UnionWardID = villageArea.UnionWardVillageAreaMap.UnionWardID;
            saveUnionWardVillageAreaMap.VillageAreaID = saveVillageArea.VillageAreaID;
            saveUnionWardVillageAreaMap.ValidityDate = DateTime.Now.AddYears(1);
            saveUnionWardVillageAreaMap.IsActive = villageArea.UnionWardVillageAreaMap.IsActive;
            saveUnionWardVillageAreaMap.CreatedBy = Convert.ToInt32(userID);
            saveUnionWardVillageAreaMap.CreatedDate = DateTime.Now;
            saveUnionWardVillageAreaMap.UpdatedBy = Convert.ToInt32(userID);
            saveUnionWardVillageAreaMap.UpdatedDate = DateTime.Now;
            _context.UnionWardVillageAreaMaps.Add(saveUnionWardVillageAreaMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(saveVillageArea.VillageAreaID);
        }
        public async Task<VMVillageArea> Update(VMVillageArea villageArea)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var updateVillageArea = _context.VillageAreas.SingleOrDefault(x => x.VillageAreaID == villageArea.VillageAreaID);
            if (updateVillageArea == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.VillageAreas.SingleOrDefault(x => x.VillageAreaCode == villageArea.VillageAreaCode && x.VillageAreaID != villageArea.VillageAreaID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }
            _context.Database.BeginTransaction();
            updateVillageArea.VillageAreaName = villageArea.VillageAreaName;
            updateVillageArea.VillageAreaCode = villageArea.VillageAreaCode;
            updateVillageArea.VillageAreaNameBangla = villageArea.VillageAreaNameBangla;
            updateVillageArea.GeoFenceID = villageArea.GeoFenceID;
            updateVillageArea.UpdatedBy = userID;
            updateVillageArea.UpdatedDate = DateTime.Now;
            _context.VillageAreas.Attach(updateVillageArea);
            _context.Entry(updateVillageArea).State = EntityState.Modified;


            var updateUnionWardVillageAreaMap = _context.UnionWardVillageAreaMaps.SingleOrDefault(x => x.UnionWardVillageAreaMapID == villageArea.UnionWardVillageAreaMap.UnionWardVillageAreaMapID);
            if (updateUnionWardVillageAreaMap != null)
            {
                updateUnionWardVillageAreaMap.UnionWardID = villageArea.UnionWardVillageAreaMap.UnionWardID;
                updateUnionWardVillageAreaMap.VillageAreaID = updateVillageArea.VillageAreaID;
                updateUnionWardVillageAreaMap.ValidityDate = villageArea.UnionWardVillageAreaMap.ValidityDate;
                updateUnionWardVillageAreaMap.IsActive = villageArea.UnionWardVillageAreaMap.IsActive;
                updateUnionWardVillageAreaMap.UpdatedBy = Convert.ToInt32(userID);
                updateUnionWardVillageAreaMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateUnionWardVillageAreaMap);
                _context.Entry(updateUnionWardVillageAreaMap).State = EntityState.Modified;
            }
            else
            {
                UnionWardVillageAreaMap saveUnionWardVillageAreaMap = new UnionWardVillageAreaMap();
                saveUnionWardVillageAreaMap.UnionWardID = villageArea.UnionWardVillageAreaMap.UnionWardID;
                saveUnionWardVillageAreaMap.VillageAreaID = updateVillageArea.VillageAreaID;
                saveUnionWardVillageAreaMap.ValidityDate = DateTime.Now.AddYears(1);
                saveUnionWardVillageAreaMap.IsActive = villageArea.UnionWardVillageAreaMap.IsActive;
                saveUnionWardVillageAreaMap.CreatedBy = Convert.ToInt32(userID);
                saveUnionWardVillageAreaMap.CreatedDate = DateTime.Now;
                saveUnionWardVillageAreaMap.UpdatedBy = Convert.ToInt32(userID);
                saveUnionWardVillageAreaMap.UpdatedDate = DateTime.Now;
                _context.UnionWardVillageAreaMaps.Add(saveUnionWardVillageAreaMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updateVillageArea.VillageAreaID);
        }
        public async Task<bool> Delete(int VillageAreaID)
        {
            var data = _context.VillageAreas.SingleOrDefault(x => x.VillageAreaID == VillageAreaID);
            if (data != null)
            {
                var map = _context.UnionWardVillageAreaMaps.SingleOrDefault(map => map.VillageAreaID == VillageAreaID);
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
        public async Task<IEnumerable<VMVillageArea>> UpdateOrder(List<VMVillageArea> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMVillageArea VillageArea in oList)
            {
                var map = _context.UnionWardVillageAreaMaps.SingleOrDefault(x => x.UnionWardVillageAreaMapID == VillageArea.UnionWardVillageAreaMap.UnionWardVillageAreaMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.UnionWardVillageAreaMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMVillageArea>> Search(QueryObject queryObject)
        {
            var mapList = _context.UnionWardVillageAreaMaps.ToList();
            if (queryObject.UnionWardID > 0)
            {
                mapList = mapList.Where(x => x.UnionWardID == queryObject.UnionWardID).ToList();
            }

            var query =
                from map in mapList
                join VillageArea in _context.VillageAreas on map.VillageAreaID equals VillageArea.VillageAreaID
                join UnionWard in _context.UnionWards on map.UnionWardID equals UnionWard.UnionWardID

                select new VMVillageArea
                {
                    VillageAreaID = VillageArea.VillageAreaID,
                    VillageAreaCode = VillageArea.VillageAreaCode,
                    VillageAreaName = VillageArea.VillageAreaName,
                    VillageAreaNameBangla = VillageArea.VillageAreaNameBangla,
                    GeoFenceID = 0,
                    UnionWardVillageAreaMap = map,
                    UnionWardName = UnionWard.UnionWardName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMVillageArea>> GetListByUnionWard(List<VMUnionWard> objList)
        {
            return new List<VMVillageArea>();
            //int selectedOrganizationID = objList[0].OrganizationUnionWardMap.OrganizationID;
            //IEnumerable<string> sUnionWardIDs = objList.Select(x => x.UnionWardID.ToString()).ToList();
            //var lstMap = _context.UnionWardVillageAreaMaps.Where(x => sUnionWardIDs.Contains(x.UnionWardID.ToString())).GroupBy(x => x.VillageAreaID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.UnionWardVillageAreaMaps.Where(x => sUnionWardIDs.Contains(x.UnionWardID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].UnionWardID == lstMapOrganization[j].UnionWardID && lstMap[i].VillageAreaID == lstMapOrganization[j].VillageAreaID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from VillageArea in _context.VillageAreas.ToList()
            //join map in lstMap on VillageArea.VillageAreaID equals map.VillageAreaID
            //join UnionWard in objList on map.UnionWardID equals UnionWard.UnionWardID

            //select new VMVillageArea
            //{
            //    VillageAreaID = VillageArea.VillageAreaID,
            //    VillageAreaCode = VillageArea.VillageAreaCode,
            //    VillageAreaName = VillageArea.VillageAreaName,
            //    VillageAreaNameBangla = VillageArea.VillageAreaNameBangla,
            //    GeoFenceID = 0,
            //    UnionWardVillageAreaMap = map,
            //    UnionWardName = UnionWard.UnionWardName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMVillageArea>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sUnionIDs = _context.OrganizationUnionWardMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.UnionWardID.ToString()).ToList();
            var lstMap = _context.UnionWardVillageAreaMaps.Where(x => sUnionIDs.Contains(x.UnionWardID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationVillageAreaMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join village in _context.VillageAreas on map.VillageAreaID equals village.VillageAreaID
                join union in _context.UnionWards on map.UnionWardID equals union.UnionWardID

                join mapOrganization in lstMapOrganization on map.VillageAreaID equals mapOrganization.VillageAreaID into table
                from temp in table.DefaultIfEmpty()
                select new VMVillageArea
                {
                    VillageAreaID = village.VillageAreaID,
                    VillageAreaCode = village.VillageAreaCode,
                    VillageAreaName = village.VillageAreaName,
                    VillageAreaNameBangla = village.VillageAreaNameBangla,
                    GeoFenceID = village.GeoFenceID,
                    UnionWardName = union.UnionWardName,
                    UnionWardVillageAreaMap = map,
                    OrganizationVillageAreaMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x => x.UnionWardName).ToList();
            return await Task.FromResult(retDataList);

        }
        public async Task<bool> SaveOrganizationVillageAreaMap(List<OrganizationVillageAreaMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            IEnumerable<string> sOrganizationIDs = oList.Select(x => x.OrganizationID.ToString()).ToList();
            IEnumerable<string> sVillageIDs = oList.Select(x => x.VillageAreaID.ToString()).ToList();
            var dbMapList = _context.OrganizationVillageAreaMaps.Where(x => sOrganizationIDs.Contains(x.OrganizationID.ToString()) && sVillageIDs.Contains(x.VillageAreaID.ToString())).ToList();

            for (int i = 0; i < oList.Count; i++)
            {
                var dbData = dbMapList.SingleOrDefault(x => x.OrganizationID == oList[i].OrganizationID && x.VillageAreaID == oList[i].VillageAreaID);
                if (dbData == null)
                {
                    if (oList[i].IsActive == true)
                    {
                        OrganizationVillageAreaMap obj = new OrganizationVillageAreaMap();
                        obj.OrganizationID = oList[i].OrganizationID;
                        obj.VillageAreaID = oList[i].VillageAreaID;
                        obj.OrderNo = oList[i].OrderNo;
                        obj.IsActive = true;
                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;
                        _context.OrganizationVillageAreaMaps.Add(obj);
                    }
                }
                else
                {
                    dbData.IsActive = oList[i].IsActive;
                    dbData.OrderNo = oList[i].OrderNo;
                    dbData.UpdatedBy = userId;
                    dbData.UpdatedDate = DateTime.Now;
                    _context.OrganizationVillageAreaMaps.Attach(dbData);
                    _context.Entry(dbData).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return true;

        }
    }
}

