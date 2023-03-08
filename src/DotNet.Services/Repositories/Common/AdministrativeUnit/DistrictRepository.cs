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
    public class DistrictRepository : IDistrictRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public DistrictRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMDistrict>> GetAll()
        {
            var query =
                from district in _context.Districts.ToList()
                join map in _context.DivisionDistrictMaps.ToList() on district.DistrictID equals map.DistrictID 
                join division in _context.Divisions.ToList() on map.DivisionID equals division.DivisionID 
                join mapCountry in _context.CountryDivisionMaps.ToList() on map.DivisionID equals mapCountry.DivisionID
                join country in _context.Countrys.ToList() on mapCountry.CountryID equals country.CountryID

                select new VMDistrict
                {
                    DistrictID = district.DistrictID,
                    DistrictCode = district.DistrictCode,
                    DistrictName = district.DistrictName,
                    DistrictNameBangla = district.DistrictNameBangla,
                    GeoFenceID = 0,
                    DivisionDistrictMap = map,
                    DivisionName = division.DivisionName,
                    CountryID = mapCountry.CountryID,
                    CountryName= country.CountryName,
                };
            var retDataList = query.OrderBy(x=>x.CountryName).ThenBy(x => x.DivisionName).ThenBy(x => x.DistrictName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMDistrict> GetByID(int id)
        {
            var result = _context.Districts.SingleOrDefault(x => x.DistrictID == id);
            var resultMap = _context.DivisionDistrictMaps.SingleOrDefault(x => x.DistrictID == id);

            VMDistrict retObj = new VMDistrict();
            retObj.DistrictID = result.DistrictID;
            retObj.DistrictName = result.DistrictName;
            retObj.DistrictCode = result.DistrictCode;
            retObj.DistrictNameBangla = result.DistrictNameBangla;
            retObj.GeoFenceID = result.GeoFenceID;
            retObj.DivisionDistrictMap = resultMap;

            return await Task.FromResult(retObj);
        }
        public async Task<VMDistrict> Add(VMDistrict district)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.Districts.SingleOrDefault(x => x.DistrictCode == district.DistrictCode);
            if(existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            District saveDistrict = new District();
            saveDistrict.DistrictName = district.DistrictName;
            saveDistrict.DistrictCode = district.DistrictCode;
            saveDistrict.DistrictNameBangla = district.DistrictNameBangla;
            saveDistrict.GeoFenceID = district.GeoFenceID;
            saveDistrict.CreatedBy = Convert.ToInt32(userID);
            saveDistrict.CreatedDate = DateTime.Now;
            saveDistrict.UpdatedBy = Convert.ToInt32(userID);
            saveDistrict.UpdatedDate = DateTime.Now;
            _context.Districts.Add(saveDistrict);
            _context.SaveChanges();

            DivisionDistrictMap saveDivisionDistrictMap = new DivisionDistrictMap();
            saveDivisionDistrictMap.DivisionID = district.DivisionDistrictMap.DivisionID;
            saveDivisionDistrictMap.DistrictID = saveDistrict.DistrictID;
            saveDivisionDistrictMap.ValidityDate = DateTime.Now.AddYears(1);
            saveDivisionDistrictMap.IsActive = district.DivisionDistrictMap.IsActive;
            saveDivisionDistrictMap.CreatedBy = Convert.ToInt32(userID);
            saveDivisionDistrictMap.CreatedDate = DateTime.Now;
            saveDivisionDistrictMap.UpdatedBy = Convert.ToInt32(userID);
            saveDivisionDistrictMap.UpdatedDate = DateTime.Now;
            _context.DivisionDistrictMaps.Add(saveDivisionDistrictMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(saveDistrict.DistrictID);
        }
        public async Task<VMDistrict> Update(VMDistrict district)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();

            var updateDistrict = _context.Districts.SingleOrDefault(x => x.DistrictID == district.DistrictID);
            if (updateDistrict == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.Districts.SingleOrDefault(x => x.DistrictCode == district.DistrictCode && x.DistrictID != district.DistrictID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }
            _context.Database.BeginTransaction();
            updateDistrict.DistrictName = district.DistrictName;
            updateDistrict.DistrictCode = district.DistrictCode;
            updateDistrict.DistrictNameBangla = district.DistrictNameBangla;
            updateDistrict.GeoFenceID = district.GeoFenceID;
            updateDistrict.UpdatedBy = userID;
            updateDistrict.UpdatedDate = DateTime.Now;
            _context.Districts.Attach(updateDistrict);
            _context.Entry(updateDistrict).State = EntityState.Modified;

            var updateDivisionDistrictMap = _context.DivisionDistrictMaps.SingleOrDefault(x => x.DistrictID == district.DistrictID);
            if (updateDivisionDistrictMap != null)
            {
                updateDivisionDistrictMap.DivisionID = district.DivisionDistrictMap.DivisionID;
                updateDivisionDistrictMap.DistrictID = updateDistrict.DistrictID;
                updateDivisionDistrictMap.ValidityDate = district.DivisionDistrictMap.ValidityDate;
                updateDivisionDistrictMap.IsActive = district.DivisionDistrictMap.IsActive;
                updateDivisionDistrictMap.UpdatedBy = Convert.ToInt32(userID);
                updateDivisionDistrictMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateDivisionDistrictMap);
                _context.Entry(updateDivisionDistrictMap).State = EntityState.Modified;
            }
            else
            {
                DivisionDistrictMap saveDivisionDistrictMap = new DivisionDistrictMap();
                saveDivisionDistrictMap.DivisionID = district.DivisionDistrictMap.DivisionID;
                saveDivisionDistrictMap.DistrictID = updateDistrict.DistrictID;
                saveDivisionDistrictMap.ValidityDate = DateTime.Now.AddYears(1);
                saveDivisionDistrictMap.IsActive = district.DivisionDistrictMap.IsActive;
                saveDivisionDistrictMap.CreatedBy = Convert.ToInt32(userID);
                saveDivisionDistrictMap.CreatedDate = DateTime.Now;
                saveDivisionDistrictMap.UpdatedBy = Convert.ToInt32(userID);
                saveDivisionDistrictMap.UpdatedDate = DateTime.Now;
                _context.DivisionDistrictMaps.Add(saveDivisionDistrictMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updateDistrict.DistrictID);
        }
        public async Task<bool> Delete(int DistrictID)
        {
            var data = _context.Districts.SingleOrDefault(x => x.DistrictID == DistrictID);
            if (data != null)
            {
                var map = _context.DivisionDistrictMaps.SingleOrDefault(map => map.DistrictID == DistrictID);
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
        public async Task<IEnumerable<VMDistrict>> UpdateOrder(List<VMDistrict> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMDistrict District in oList)
            {
                var map = _context.DivisionDistrictMaps.SingleOrDefault(x => x.DivisionDistrictMapID == District.DivisionDistrictMap.DivisionDistrictMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.DivisionDistrictMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMDistrict>> Search(QueryObject queryObject)
        {
            var mapList = _context.DivisionDistrictMaps.ToList();
            if (queryObject.DivisionID > 0)
            {
                mapList = mapList.Where(x => x.DivisionID == queryObject.DivisionID).ToList();
            }

            var query =
                from map in mapList
                join District in _context.Districts on map.DistrictID equals District.DistrictID
                join Division in _context.Divisions on map.DivisionID equals Division.DivisionID

                select new VMDistrict
                {
                    DistrictID = District.DistrictID,
                    DistrictCode = District.DistrictCode,
                    DistrictName = District.DistrictName,
                    DistrictNameBangla = District.DistrictNameBangla,
                    GeoFenceID = 0,
                    DivisionDistrictMap = map,
                    DivisionName = Division.DivisionName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMDistrict>> GetListByDivision(List<VMDivision> objList)
        {
            return new List<VMDistrict>();
            //int selectedOrganizationID = objList[0].OrganizationDivisionMap.OrganizationID;
            //IEnumerable<string> sDivisionIDs = objList.Select(x => x.DivisionID.ToString()).ToList();
            //var lstMap = _context.DivisionDistrictMaps.Where(x => sDivisionIDs.Contains(x.DivisionID.ToString())).GroupBy(x => x.DistrictID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.DivisionDistrictMaps.Where(x => sDivisionIDs.Contains(x.DivisionID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].DivisionID == lstMapOrganization[j].DivisionID && lstMap[i].DistrictID == lstMapOrganization[j].DistrictID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from District in _context.Districts.ToList()
            //join map in lstMap on District.DistrictID equals map.DistrictID
            //join Division in objList on map.DivisionID equals Division.DivisionID

            //select new VMDistrict
            //{
            //    DistrictID = District.DistrictID,
            //    DistrictCode = District.DistrictCode,
            //    DistrictName = District.DistrictName,
            //    DistrictNameBangla = District.DistrictNameBangla,
            //    GeoFenceID = 0,
            //    DivisionDistrictMap = map,
            //    DivisionName = Division.DivisionName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMDistrict>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sDivisionIDs = _context.OrganizationDivisionMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.DivisionID.ToString()).ToList();
            var lstMap = _context.DivisionDistrictMaps.Where(x => sDivisionIDs.Contains(x.DivisionID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationDistrictMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join district in _context.Districts on map.DistrictID equals district.DistrictID
                join division in _context.Divisions on map.DivisionID equals division.DivisionID

                join mapOrganization in lstMapOrganization on map.DistrictID equals mapOrganization.DistrictID into table
                from temp in table.DefaultIfEmpty()
                select new VMDistrict
                {
                    DistrictID = district.DistrictID,
                    DistrictCode = district.DistrictCode,
                    DistrictName = district.DistrictNameBangla,
                    DistrictNameBangla = district.DistrictNameBangla,
                    GeoFenceID = district.GeoFenceID,
                    DivisionName = division.DivisionName,

                    DivisionDistrictMap = map,
                    OrganizationDistrictMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x => x.DivisionName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<bool> SaveOrganizationDistrictMap(List<OrganizationDistrictMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            _context.Database.BeginTransaction();

            var newDataList = oList.Where(x => x.OrganizationDistrictMapID == 0).ToList();
            var oldData = oList.Where(x => x.OrganizationDistrictMapID > 0).ToList();
            IEnumerable<string> sOldDatas = oldData.Select(x => x.OrganizationDistrictMapID.ToString()).ToList();
            var dbOldDataList = _context.OrganizationDistrictMaps.Where(x => sOldDatas.Contains(x.OrganizationDistrictMapID.ToString())).ToList();

            for (int i = 0; i < newDataList.Count; i++)
            {
                OrganizationDistrictMap obj = new OrganizationDistrictMap();
                obj.OrganizationID = newDataList[i].OrganizationID;
                obj.DistrictID = newDataList[i].DistrictID;
                obj.IsActive = true;
                obj.OrderNo = newDataList[i].OrderNo;
                obj.CreatedBy = userId;
                obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = userId;
                obj.UpdatedDate = DateTime.Now;
                _context.OrganizationDistrictMaps.Add(obj);
            }
            for (int i = 0; i < oldData.Count; i++)
            {
                var obj = dbOldDataList.SingleOrDefault(x => x.OrganizationDistrictMapID == oldData[i].OrganizationDistrictMapID);
                if (obj != null)
                {
                    obj.IsActive = oldData[i].IsActive;
                    obj.OrderNo = oldData[i].OrderNo;
                    obj.UpdatedBy = userId;
                    obj.UpdatedDate = DateTime.Now;
                    _context.Entry(obj).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();
            return true;
        }
    }
}

