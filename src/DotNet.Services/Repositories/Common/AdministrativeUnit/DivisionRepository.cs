using Microsoft.AspNetCore.Http;
using DotNet.ApplicationCore.Utils.Helper;
using DotNet.Infrastructure.Persistence.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;





using DotNet.ApplicationCore.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Utils.Enum;


using DotNet.ApplicationCore.Entities;
using System.Diagnostics.Metrics;



namespace DotNet.Services.Repositories.Common.AdministrativeUnit
{
    public class DivisionRepository : IDivisionRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public DivisionRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMDivision>> GetAll()
        {
            var query =
                from division in _context.Divisions.ToList()
                join map in _context.CountryDivisionMaps.ToList() on division.DivisionID equals map.DivisionID
                join country in _context.Countrys.ToList() on map.CountryID equals country.CountryID

                select new VMDivision
                {
                    DivisionID = division.DivisionID,
                    DivisionCode = division.DivisionCode,
                    DivisionName = division.DivisionName,
                    DivisionNameBangla = division.DivisionNameBangla,
                    GeoFenceID = 0,
                    CountryDivisionMap = map,
                    CountryName = country.CountryName,
                };

            var retDataList = query.OrderBy(x => x.CountryName).ThenBy(x => x.DivisionName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMDivision> GetByID(int id)
        {
            var result = _context.Divisions.SingleOrDefault(x => x.DivisionID == id);
            var resultMap = _context.CountryDivisionMaps.SingleOrDefault(x => x.DivisionID == id);

            VMDivision retObj = new VMDivision();
            retObj.DivisionID = result.DivisionID;
            retObj.DivisionName = result.DivisionName;
            retObj.DivisionCode = result.DivisionCode;
            retObj.DivisionNameBangla = result.DivisionNameBangla;
            retObj.GeoFenceID = result.GeoFenceID;
            retObj.CountryDivisionMap = resultMap;

            return await Task.FromResult(retObj);
        }
        public async Task<VMDivision> Add(VMDivision division)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.Divisions.SingleOrDefault(x => x.DivisionCode == division.DivisionCode);
            if (existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            Division saveDivision = new Division();
            saveDivision.DivisionName = division.DivisionName;
            saveDivision.DivisionCode = division.DivisionCode;
            saveDivision.DivisionNameBangla = division.DivisionNameBangla;
            saveDivision.GeoFenceID = division.GeoFenceID;
            saveDivision.CreatedBy = Convert.ToInt32(userID);
            saveDivision.CreatedDate = DateTime.Now;
            saveDivision.UpdatedBy = Convert.ToInt32(userID);
            saveDivision.UpdatedDate = DateTime.Now;
            _context.Divisions.Add(saveDivision);
            _context.SaveChanges();

            CountryDivisionMap saveCountryDivisionMap = new CountryDivisionMap();
            saveCountryDivisionMap.CountryID = division.CountryDivisionMap.CountryID;
            saveCountryDivisionMap.DivisionID = saveDivision.DivisionID;
            saveCountryDivisionMap.ValidityDate = DateTime.Now.AddYears(1);
            saveCountryDivisionMap.IsActive = division.CountryDivisionMap.IsActive;
            saveCountryDivisionMap.CreatedBy = Convert.ToInt32(userID);
            saveCountryDivisionMap.CreatedDate = DateTime.Now;
            saveCountryDivisionMap.UpdatedBy = Convert.ToInt32(userID);
            saveCountryDivisionMap.UpdatedDate = DateTime.Now;
            _context.CountryDivisionMaps.Add(saveCountryDivisionMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(saveDivision.DivisionID);
        }
        public async Task<VMDivision> Update(VMDivision division)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();

            var updateDivision = _context.Divisions.SingleOrDefault(x => x.DivisionID == division.DivisionID);
            if (updateDivision == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.Divisions.SingleOrDefault(x => x.DivisionCode == division.DivisionCode && x.DivisionID != division.DivisionID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }
            _context.Database.BeginTransaction();
            updateDivision.DivisionName = division.DivisionName;
            updateDivision.DivisionCode = division.DivisionCode;
            updateDivision.DivisionNameBangla = division.DivisionNameBangla;
            updateDivision.GeoFenceID = division.GeoFenceID;
            updateDivision.UpdatedBy = userID;
            updateDivision.UpdatedDate = DateTime.Now;
            _context.Divisions.Attach(updateDivision);
            _context.Entry(updateDivision).State = EntityState.Modified;

            var updateCountryDivisionMap = _context.CountryDivisionMaps.SingleOrDefault(x => x.DivisionID == division.DivisionID);
            if (updateCountryDivisionMap != null)
            {
                updateCountryDivisionMap.CountryID = division.CountryDivisionMap.CountryID;
                updateCountryDivisionMap.DivisionID = updateDivision.DivisionID;
                updateCountryDivisionMap.ValidityDate = division.CountryDivisionMap.ValidityDate;
                updateCountryDivisionMap.IsActive = division.CountryDivisionMap.IsActive;
                updateCountryDivisionMap.UpdatedBy = Convert.ToInt32(userID);
                updateCountryDivisionMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateCountryDivisionMap);
                _context.Entry(updateCountryDivisionMap).State = EntityState.Modified;
            }
            else
            {
                CountryDivisionMap saveCountryDivisionMap = new CountryDivisionMap();
                saveCountryDivisionMap.CountryID = division.CountryDivisionMap.CountryID;
                saveCountryDivisionMap.DivisionID = updateDivision.DivisionID;
                saveCountryDivisionMap.ValidityDate = DateTime.Now.AddYears(1);
                saveCountryDivisionMap.IsActive = division.CountryDivisionMap.IsActive;
                saveCountryDivisionMap.CreatedBy = Convert.ToInt32(userID);
                saveCountryDivisionMap.CreatedDate = DateTime.Now;
                saveCountryDivisionMap.UpdatedBy = Convert.ToInt32(userID);
                saveCountryDivisionMap.UpdatedDate = DateTime.Now;
                _context.CountryDivisionMaps.Add(saveCountryDivisionMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updateDivision.DivisionID);
        }
        public async Task<bool> Delete(int divisionID)
        {
            var data = _context.Divisions.SingleOrDefault(x => x.DivisionID == divisionID);
            if (data != null)
            {
                var divisionMap = _context.DivisionDistrictMaps.FirstOrDefault(x => x.DivisionID == data.DivisionID);
                if (divisionMap != null)
                {
                    throw new Exception("Please delete its district first!");
                }

                var map = _context.CountryDivisionMaps.SingleOrDefault(map => map.DivisionID == divisionID);
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
        public async Task<IEnumerable<VMDivision>> UpdateOrder(List<VMDivision> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMDivision division in oList)
            {
                var map = _context.CountryDivisionMaps.SingleOrDefault(x => x.CountryDivisionMapID == division.CountryDivisionMap.CountryDivisionMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.CountryDivisionMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMDivision>> Search(QueryObject queryObject)
        {
            var mapList = _context.CountryDivisionMaps.ToList();
            if (queryObject.CountryID > 0)
            {
                mapList = mapList.Where(x => x.CountryID == queryObject.CountryID).ToList();
            }

            var query =
                from map in mapList
                join division in _context.Divisions on map.DivisionID equals division.DivisionID
                join country in _context.Countrys on map.CountryID equals country.CountryID

                select new VMDivision
                {
                    DivisionID = division.DivisionID,
                    DivisionCode = division.DivisionCode,
                    DivisionName = division.DivisionName,
                    DivisionNameBangla = division.DivisionNameBangla,
                    GeoFenceID = 0,
                    CountryDivisionMap = map,
                    CountryName = country.CountryName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMDivision>> GetListByCountry(List<VMCountry> objList)
        {
            return new List<VMDivision>();
            //int selectedOrganizationID = objList[0].OrganizationCountryMap.OrganizationID;
            //IEnumerable<string> sCountryIDs = objList.Select(x => x.CountryID.ToString()).ToList();
            //var lstMap = _context.CountryDivisionMaps.Where(x => sCountryIDs.Contains(x.CountryID.ToString())).GroupBy(x => x.DivisionID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.CountryDivisionMaps.Where(x => sCountryIDs.Contains(x.CountryID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].CountryID == lstMapOrganization[j].CountryID && lstMap[i].DivisionID == lstMapOrganization[j].DivisionID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from division in _context.Divisions.ToList()
            //join map in lstMap on division.DivisionID equals map.DivisionID
            //join country in objList on map.CountryID equals country.CountryID

            //select new VMDivision
            //{
            //    DivisionID = division.DivisionID,
            //    DivisionCode = division.DivisionCode,
            //    DivisionName = division.DivisionName,
            //    DivisionNameBangla = division.DivisionNameBangla,
            //    GeoFenceID = 0,
            //    CountryDivisionMap = map,
            //    CountryName = country.CountryName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMDivision>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sCountryIDs = _context.OrganizationCountryMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.CountryID.ToString()).ToList();
            var lstMap = _context.CountryDivisionMaps.Where(x => sCountryIDs.Contains(x.CountryID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationDivisionMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join division in _context.Divisions on map.DivisionID equals division.DivisionID
                join country in _context.Countrys on map.CountryID equals country.CountryID

                join mapOrganization in lstMapOrganization on map.DivisionID equals mapOrganization.DivisionID into table
                from temp in table.DefaultIfEmpty()
                select new VMDivision
                {
                    DivisionID = division.DivisionID,
                    DivisionCode = division.DivisionCode,
                    DivisionName = division.DivisionName,
                    DivisionNameBangla = division.DivisionNameBangla,
                    GeoFenceID = division.GeoFenceID,
                    CountryName = country.CountryName,
                    CountryDivisionMap = map,
                    OrganizationDivisionMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x=>x.OrderNo).ThenBy(x => x.DivisionName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<bool> SaveOrganizationDivisionMap(List<OrganizationDivisionMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            _context.Database.BeginTransaction();

            var newDataList = oList.Where(x => x.OrganizationDivisionMapID == 0).ToList();
            var oldData = oList.Where(x => x.OrganizationDivisionMapID > 0).ToList();
            IEnumerable<string> sOldDatas = oldData.Select(x => x.OrganizationDivisionMapID.ToString()).ToList();
            var dbOldDataList = _context.OrganizationDivisionMaps.Where(x => sOldDatas.Contains(x.OrganizationDivisionMapID.ToString())).ToList();

            for (int i = 0; i < newDataList.Count; i++)
            {
                OrganizationDivisionMap obj = new OrganizationDivisionMap();
                obj.OrganizationID = newDataList[i].OrganizationID;
                obj.DivisionID = newDataList[i].DivisionID;
                obj.IsActive = true;
                obj.OrderNo = newDataList[i].OrderNo;
                obj.CreatedBy = userId;
                obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = userId;
                obj.UpdatedDate = DateTime.Now;
                _context.OrganizationDivisionMaps.Add(obj);
            }
            for (int i = 0; i < oldData.Count; i++)
            {
                var obj = dbOldDataList.SingleOrDefault(x => x.OrganizationDivisionMapID == oldData[i].OrganizationDivisionMapID);
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

