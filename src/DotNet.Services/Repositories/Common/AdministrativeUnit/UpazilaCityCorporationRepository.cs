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
    public class UpazilaCityCorporationRepository : IUpazilaCityCorporationRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpazilaCityCorporationRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMUpazilaCityCorporation>> GetAll()
        {
            var query =
                from upazilaCityCorporation in _context.UpazilaCityCorporations.ToList()
                join map in _context.DistrictUpazilaCityCorporationMaps.ToList() on upazilaCityCorporation.UpazilaCityCorporationID equals map.UpazilaCityCorporationID
                join district in _context.Districts.ToList() on map.DistrictID equals district.DistrictID

                select new VMUpazilaCityCorporation
                {
                    UpazilaCityCorporationID = upazilaCityCorporation.UpazilaCityCorporationID,
                    UpazilaCityCorporationCode = upazilaCityCorporation.UpazilaCityCorporationCode,
                    UpazilaCityCorporationName = upazilaCityCorporation.UpazilaCityCorporationName,
                    UpazilaCityCorporationNameBangla = upazilaCityCorporation.UpazilaCityCorporationNameBangla,
                    GeoFenceID = upazilaCityCorporation.GeoFenceID,
                    IsUpazila = upazilaCityCorporation.IsUpazila,
                    DistrictUpazilaCityCorporationMap = map,
                    DistrictName = district.DistrictName
                };
            var retDataList = query.OrderBy(x => x.DistrictName).ThenBy(x => x.UpazilaCityCorporationName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMUpazilaCityCorporation> GetByID(int id)
        {
            var result = _context.UpazilaCityCorporations.SingleOrDefault(x => x.UpazilaCityCorporationID == id);
            var resultMap = _context.DistrictUpazilaCityCorporationMaps.SingleOrDefault(x => x.UpazilaCityCorporationID == id);

            VMUpazilaCityCorporation retObj = new VMUpazilaCityCorporation()
            {
                UpazilaCityCorporationID = result.UpazilaCityCorporationID,
                UpazilaCityCorporationName = result.UpazilaCityCorporationName,
                UpazilaCityCorporationCode = result.UpazilaCityCorporationCode,
                UpazilaCityCorporationNameBangla = result.UpazilaCityCorporationNameBangla,
                GeoFenceID = result.GeoFenceID,
                IsUpazila = result.IsUpazila,
                DistrictUpazilaCityCorporationMap = resultMap,
            };
            return await Task.FromResult(retObj);
        }
        public async Task<VMUpazilaCityCorporation> Add(VMUpazilaCityCorporation upazilaCityCorporation)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.UpazilaCityCorporations.SingleOrDefault(x => x.UpazilaCityCorporationCode == upazilaCityCorporation.UpazilaCityCorporationCode);
            if (existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            UpazilaCityCorporation saveUpazilaCityCorporation = new UpazilaCityCorporation();
            saveUpazilaCityCorporation.UpazilaCityCorporationName = upazilaCityCorporation.UpazilaCityCorporationName;
            saveUpazilaCityCorporation.UpazilaCityCorporationCode = upazilaCityCorporation.UpazilaCityCorporationCode;
            saveUpazilaCityCorporation.UpazilaCityCorporationNameBangla = upazilaCityCorporation.UpazilaCityCorporationNameBangla;
            saveUpazilaCityCorporation.GeoFenceID = upazilaCityCorporation.GeoFenceID;
            saveUpazilaCityCorporation.IsUpazila = upazilaCityCorporation.IsUpazila;
            saveUpazilaCityCorporation.CreatedBy = Convert.ToInt32(userID);
            saveUpazilaCityCorporation.CreatedDate = DateTime.Now;
            saveUpazilaCityCorporation.UpdatedBy = Convert.ToInt32(userID);
            saveUpazilaCityCorporation.UpdatedDate = DateTime.Now;
            _context.UpazilaCityCorporations.Add(saveUpazilaCityCorporation);
            _context.SaveChanges();

            DistrictUpazilaCityCorporationMap saveDistrictUpazilaCityCorporationMap = new DistrictUpazilaCityCorporationMap();
            saveDistrictUpazilaCityCorporationMap.DistrictID = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.DistrictID;
            saveDistrictUpazilaCityCorporationMap.UpazilaCityCorporationID = saveUpazilaCityCorporation.UpazilaCityCorporationID;
            saveDistrictUpazilaCityCorporationMap.ValidityDate = DateTime.Now.AddYears(1);
            saveDistrictUpazilaCityCorporationMap.IsActive = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.IsActive;
            saveDistrictUpazilaCityCorporationMap.CreatedBy = Convert.ToInt32(userID);
            saveDistrictUpazilaCityCorporationMap.CreatedDate = DateTime.Now;
            saveDistrictUpazilaCityCorporationMap.UpdatedBy = Convert.ToInt32(userID);
            saveDistrictUpazilaCityCorporationMap.UpdatedDate = DateTime.Now;
            _context.DistrictUpazilaCityCorporationMaps.Add(saveDistrictUpazilaCityCorporationMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(saveUpazilaCityCorporation.UpazilaCityCorporationID);
        }
        public async Task<VMUpazilaCityCorporation> Update(VMUpazilaCityCorporation upazilaCityCorporation)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            var updateUpazilaCityCorporation = _context.UpazilaCityCorporations.SingleOrDefault(x => x.UpazilaCityCorporationID == upazilaCityCorporation.UpazilaCityCorporationID);
            if (updateUpazilaCityCorporation == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.UpazilaCityCorporations.SingleOrDefault(x => x.UpazilaCityCorporationCode == upazilaCityCorporation.UpazilaCityCorporationCode && x.UpazilaCityCorporationID != upazilaCityCorporation.UpazilaCityCorporationID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }
            _context.Database.BeginTransaction();
            updateUpazilaCityCorporation.UpazilaCityCorporationName = upazilaCityCorporation.UpazilaCityCorporationName;
            updateUpazilaCityCorporation.UpazilaCityCorporationCode = upazilaCityCorporation.UpazilaCityCorporationCode;
            updateUpazilaCityCorporation.UpazilaCityCorporationNameBangla = upazilaCityCorporation.UpazilaCityCorporationNameBangla;
            updateUpazilaCityCorporation.GeoFenceID = upazilaCityCorporation.GeoFenceID;
            updateUpazilaCityCorporation.IsUpazila = upazilaCityCorporation.IsUpazila;
            updateUpazilaCityCorporation.UpdatedBy = userID;
            updateUpazilaCityCorporation.UpdatedDate = DateTime.Now;
            _context.UpazilaCityCorporations.Attach(updateUpazilaCityCorporation);
            _context.Entry(updateUpazilaCityCorporation).State = EntityState.Modified;

            var updateDistrictUpazilaCityCorporationMap = _context.DistrictUpazilaCityCorporationMaps.SingleOrDefault(x => x.UpazilaCityCorporationID == upazilaCityCorporation.UpazilaCityCorporationID);
            if (updateDistrictUpazilaCityCorporationMap != null)
            {
                updateDistrictUpazilaCityCorporationMap.DistrictID = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.DistrictID;
                updateDistrictUpazilaCityCorporationMap.UpazilaCityCorporationID = updateUpazilaCityCorporation.UpazilaCityCorporationID;
                updateDistrictUpazilaCityCorporationMap.ValidityDate = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.ValidityDate;
                updateDistrictUpazilaCityCorporationMap.IsActive = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.IsActive;
                updateDistrictUpazilaCityCorporationMap.UpdatedBy = Convert.ToInt32(userID);
                updateDistrictUpazilaCityCorporationMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateDistrictUpazilaCityCorporationMap);
                _context.Entry(updateDistrictUpazilaCityCorporationMap).State = EntityState.Modified;
            }
            else
            {
                DistrictUpazilaCityCorporationMap saveDistrictUpazilaCityCorporationMap = new DistrictUpazilaCityCorporationMap();
                saveDistrictUpazilaCityCorporationMap.DistrictID = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.DistrictID;
                saveDistrictUpazilaCityCorporationMap.UpazilaCityCorporationID = updateUpazilaCityCorporation.UpazilaCityCorporationID;
                saveDistrictUpazilaCityCorporationMap.ValidityDate = DateTime.Now.AddYears(1);
                saveDistrictUpazilaCityCorporationMap.IsActive = upazilaCityCorporation.DistrictUpazilaCityCorporationMap.IsActive;
                saveDistrictUpazilaCityCorporationMap.CreatedBy = Convert.ToInt32(userID);
                saveDistrictUpazilaCityCorporationMap.CreatedDate = DateTime.Now;
                saveDistrictUpazilaCityCorporationMap.UpdatedBy = Convert.ToInt32(userID);
                saveDistrictUpazilaCityCorporationMap.UpdatedDate = DateTime.Now;
                _context.DistrictUpazilaCityCorporationMaps.Add(saveDistrictUpazilaCityCorporationMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updateUpazilaCityCorporation.UpazilaCityCorporationID);
        }
        public async Task<bool> Delete(int UpazilaCityCorporationID)
        {
            var data = _context.UpazilaCityCorporations.SingleOrDefault(x => x.UpazilaCityCorporationID == UpazilaCityCorporationID);
            if (data != null)
            {
                var map = _context.DistrictUpazilaCityCorporationMaps.SingleOrDefault(map => map.UpazilaCityCorporationID == UpazilaCityCorporationID);
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
        public async Task<IEnumerable<VMUpazilaCityCorporation>> UpdateOrder(List<VMUpazilaCityCorporation> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMUpazilaCityCorporation UpazilaCityCorporation in oList)
            {
                var map = _context.DistrictUpazilaCityCorporationMaps.SingleOrDefault(x => x.DistrictUpazilaCityCorporationMapID == UpazilaCityCorporation.DistrictUpazilaCityCorporationMap.DistrictUpazilaCityCorporationMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.DistrictUpazilaCityCorporationMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> Search(QueryObject queryObject)
        {
            var mapList = _context.DistrictUpazilaCityCorporationMaps.ToList();
            if (queryObject.DistrictID > 0)
            {
                mapList = mapList.Where(x => x.DistrictID == queryObject.DistrictID).ToList();
            }

            var query =
                from map in mapList
                join UpazilaCityCorporation in _context.UpazilaCityCorporations on map.UpazilaCityCorporationID equals UpazilaCityCorporation.UpazilaCityCorporationID
                join District in _context.Districts on map.DistrictID equals District.DistrictID

                select new VMUpazilaCityCorporation
                {
                    UpazilaCityCorporationID = UpazilaCityCorporation.UpazilaCityCorporationID,
                    UpazilaCityCorporationCode = UpazilaCityCorporation.UpazilaCityCorporationCode,
                    UpazilaCityCorporationName = UpazilaCityCorporation.UpazilaCityCorporationName,
                    UpazilaCityCorporationNameBangla = UpazilaCityCorporation.UpazilaCityCorporationNameBangla,
                    GeoFenceID = 0,
                    DistrictUpazilaCityCorporationMap = map,
                    DistrictName = District.DistrictName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> GetListByDistrict(List<VMDistrict> objList)
        {
            return new List<VMUpazilaCityCorporation>();
            //int selectedOrganizationID = objList[0].OrganizationDistrictMap.OrganizationID;
            //IEnumerable<string> sDistrictIDs = objList.Select(x => x.DistrictID.ToString()).ToList();
            //var lstMap = _context.DistrictUpazilaCityCorporationMaps.Where(x => sDistrictIDs.Contains(x.DistrictID.ToString())).GroupBy(x => x.UpazilaCityCorporationID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.DistrictUpazilaCityCorporationMaps.Where(x => sDistrictIDs.Contains(x.DistrictID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].DistrictID == lstMapOrganization[j].DistrictID && lstMap[i].UpazilaCityCorporationID == lstMapOrganization[j].UpazilaCityCorporationID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from UpazilaCityCorporation in _context.UpazilaCityCorporations.ToList()
            //join map in lstMap on UpazilaCityCorporation.UpazilaCityCorporationID equals map.UpazilaCityCorporationID
            //join District in objList on map.DistrictID equals District.DistrictID

            //select new VMUpazilaCityCorporation
            //{
            //    UpazilaCityCorporationID = UpazilaCityCorporation.UpazilaCityCorporationID,
            //    UpazilaCityCorporationCode = UpazilaCityCorporation.UpazilaCityCorporationCode,
            //    UpazilaCityCorporationName = UpazilaCityCorporation.UpazilaCityCorporationName,
            //    UpazilaCityCorporationNameBangla = UpazilaCityCorporation.UpazilaCityCorporationNameBangla,
            //    GeoFenceID = 0,
            //    DistrictUpazilaCityCorporationMap = map,
            //    DistrictName = District.DistrictName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMUpazilaCityCorporation>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sDistrictIDs = _context.OrganizationDistrictMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.DistrictID.ToString()).ToList();
            var lstMap = _context.DistrictUpazilaCityCorporationMaps.Where(x => sDistrictIDs.Contains(x.DistrictID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationUpazilaCityCorporationMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join upazila in _context.UpazilaCityCorporations on map.UpazilaCityCorporationID equals upazila.UpazilaCityCorporationID
                join district in _context.Districts on map.DistrictID equals district.DistrictID

                join mapOrganization in lstMapOrganization on map.UpazilaCityCorporationID equals mapOrganization.UpazilaCityCorporationID into table
                from temp in table.DefaultIfEmpty()
                select new VMUpazilaCityCorporation
                {
                    UpazilaCityCorporationID = upazila.UpazilaCityCorporationID,
                    UpazilaCityCorporationCode = upazila.UpazilaCityCorporationCode,
                    UpazilaCityCorporationName = upazila.UpazilaCityCorporationName,
                    UpazilaCityCorporationNameBangla = upazila.UpazilaCityCorporationNameBangla,
                    GeoFenceID = upazila.GeoFenceID,
                    DistrictName = district.DistrictName,
                    DistrictUpazilaCityCorporationMap = map,
                    OrganizationUpazilaCityCorporationMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x => x.UpazilaCityCorporationName).ToList();
            return await Task.FromResult(retDataList);

        }
        public async Task<bool> SaveOrganizationUpazilaCityCorporationMap(List<OrganizationUpazilaCityCorporationMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            IEnumerable<string> sOrganizationIDs = oList.Select(x => x.OrganizationID.ToString()).ToList();
            IEnumerable<string> sUpazilaIDs = oList.Select(x => x.UpazilaCityCorporationID.ToString()).ToList();
            var dbMapList = _context.OrganizationUpazilaCityCorporationMaps.Where(x => sOrganizationIDs.Contains(x.OrganizationID.ToString()) && sUpazilaIDs.Contains(x.UpazilaCityCorporationID.ToString())).ToList();

            for (int i = 0; i < oList.Count; i++)
            {
                var dbData = dbMapList.SingleOrDefault(x => x.OrganizationID == oList[i].OrganizationID && x.UpazilaCityCorporationID == oList[i].UpazilaCityCorporationID);
                if (dbData == null)
                {
                    if (oList[i].IsActive == true)
                    {
                        OrganizationUpazilaCityCorporationMap obj = new OrganizationUpazilaCityCorporationMap();
                        obj.OrganizationID = oList[i].OrganizationID;
                        obj.UpazilaCityCorporationID = oList[i].UpazilaCityCorporationID;
                        obj.IsActive = true;
                        obj.OrderNo = 0;
                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;
                        _context.OrganizationUpazilaCityCorporationMaps.Add(obj);
                    }
                }
                else
                {
                    dbData.IsActive = oList[i].IsActive;
                    dbData.OrderNo = oList[i].OrderNo;
                    dbData.UpdatedBy = userId;
                    dbData.UpdatedDate = DateTime.Now;
                    _context.OrganizationUpazilaCityCorporationMaps.Attach(dbData);
                    _context.Entry(dbData).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return true;
        }
    }
}

