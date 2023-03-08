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
    public class ThanaRepository : IThanaRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ThanaRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMThana>> GetAll()
        {
            var query =
                from thana in _context.Thanas.ToList()
                join map in _context.UpazilaCityCorporationThanaMaps.ToList() on thana.ThanaID equals map.ThanaID
                join upazilaCityCorporation in _context.UpazilaCityCorporations.ToList() on map.UpazilaCityCorporationID equals upazilaCityCorporation.UpazilaCityCorporationID

                select new VMThana
                {
                    ThanaID = thana.ThanaID,
                    ThanaCode = thana.ThanaCode,
                    ThanaName = thana.ThanaName,
                    ThanaNameBangla = thana.ThanaNameBangla,
                    GeoFenceID = 0,
                    UpazilaCityCorporationThanaMap = map,
                    UpazilaCityCorporationName = upazilaCityCorporation.UpazilaCityCorporationName,
                };
            var retDataList = query.OrderBy(x => x.UpazilaCityCorporationName).ThenBy(x => x.ThanaName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMThana> GetByID(int id)
        {
            var result = _context.Thanas.SingleOrDefault(x => x.ThanaID == id);
            var resultMap = _context.UpazilaCityCorporationThanaMaps.SingleOrDefault(x => x.ThanaID == id);

            VMThana retObj = new VMThana();
            retObj.ThanaID = result.ThanaID;
            retObj.ThanaName = result.ThanaName;
            retObj.ThanaCode = result.ThanaCode;
            retObj.ThanaNameBangla = result.ThanaNameBangla;
            retObj.GeoFenceID = result.GeoFenceID;
            retObj.UpazilaCityCorporationThanaMap = resultMap;

            return await Task.FromResult(retObj);
        }
        public async Task<VMThana> Add(VMThana thana)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.Thanas.SingleOrDefault(x => x.ThanaCode == thana.ThanaCode);
            if (existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            Thana saveThana = new Thana();
            saveThana.ThanaName = thana.ThanaName;
            saveThana.ThanaCode = thana.ThanaCode;
            saveThana.ThanaNameBangla = thana.ThanaNameBangla;
            saveThana.GeoFenceID = thana.GeoFenceID;
            saveThana.CreatedBy = Convert.ToInt32(userID);
            saveThana.CreatedDate = DateTime.Now;
            saveThana.UpdatedBy = Convert.ToInt32(userID);
            saveThana.UpdatedDate = DateTime.Now;
            _context.Thanas.Add(saveThana);
            _context.SaveChanges();

            UpazilaCityCorporationThanaMap saveUpazilaCityCorporationThanaMap = new UpazilaCityCorporationThanaMap();
            saveUpazilaCityCorporationThanaMap.UpazilaCityCorporationID = thana.UpazilaCityCorporationThanaMap.UpazilaCityCorporationID;
            saveUpazilaCityCorporationThanaMap.ThanaID = saveThana.ThanaID;
            saveUpazilaCityCorporationThanaMap.ValidityDate = DateTime.Now.AddYears(1);
            saveUpazilaCityCorporationThanaMap.IsActive = thana.UpazilaCityCorporationThanaMap.IsActive;
            saveUpazilaCityCorporationThanaMap.CreatedBy = Convert.ToInt32(userID);
            saveUpazilaCityCorporationThanaMap.CreatedDate = DateTime.Now;
            saveUpazilaCityCorporationThanaMap.UpdatedBy = Convert.ToInt32(userID);
            saveUpazilaCityCorporationThanaMap.UpdatedDate = DateTime.Now;
            _context.UpazilaCityCorporationThanaMaps.Add(saveUpazilaCityCorporationThanaMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(saveThana.ThanaID);
        }
        public async Task<VMThana> Update(VMThana thana)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();

            var updateThana = _context.Thanas.SingleOrDefault(x => x.ThanaID == thana.ThanaID);
            if (updateThana == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.Thanas.SingleOrDefault(x => x.ThanaCode == thana.ThanaCode && x.ThanaID!= thana.ThanaID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            updateThana.ThanaName = thana.ThanaName;
            updateThana.ThanaCode = thana.ThanaCode;
            updateThana.ThanaNameBangla = thana.ThanaNameBangla;
            updateThana.GeoFenceID = thana.GeoFenceID;
            updateThana.UpdatedBy = userID;
            updateThana.UpdatedDate = DateTime.Now;
            _context.Thanas.Attach(updateThana);
            _context.Entry(updateThana).State = EntityState.Modified;

            var updateUpazilaCityCorporationThanaMap = _context.UpazilaCityCorporationThanaMaps.SingleOrDefault(x => x.ThanaID == thana.ThanaID);
            if (updateUpazilaCityCorporationThanaMap != null)
            {
                updateUpazilaCityCorporationThanaMap.UpazilaCityCorporationID = thana.UpazilaCityCorporationThanaMap.UpazilaCityCorporationID;
                updateUpazilaCityCorporationThanaMap.ThanaID = updateThana.ThanaID;
                updateUpazilaCityCorporationThanaMap.ValidityDate = thana.UpazilaCityCorporationThanaMap.ValidityDate;
                updateUpazilaCityCorporationThanaMap.IsActive = thana.UpazilaCityCorporationThanaMap.IsActive;
                updateUpazilaCityCorporationThanaMap.UpdatedBy = Convert.ToInt32(userID);
                updateUpazilaCityCorporationThanaMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateUpazilaCityCorporationThanaMap);
                _context.Entry(updateUpazilaCityCorporationThanaMap).State = EntityState.Modified;
            }
            else
            {
                UpazilaCityCorporationThanaMap saveUpazilaCityCorporationThanaMap = new UpazilaCityCorporationThanaMap();
                saveUpazilaCityCorporationThanaMap.UpazilaCityCorporationID = thana.UpazilaCityCorporationThanaMap.UpazilaCityCorporationID;
                saveUpazilaCityCorporationThanaMap.ThanaID = updateThana.ThanaID;
                saveUpazilaCityCorporationThanaMap.ValidityDate = DateTime.Now.AddYears(1);
                saveUpazilaCityCorporationThanaMap.IsActive = thana.UpazilaCityCorporationThanaMap.IsActive;
                saveUpazilaCityCorporationThanaMap.CreatedBy = Convert.ToInt32(userID);
                saveUpazilaCityCorporationThanaMap.CreatedDate = DateTime.Now;
                saveUpazilaCityCorporationThanaMap.UpdatedBy = Convert.ToInt32(userID);
                saveUpazilaCityCorporationThanaMap.UpdatedDate = DateTime.Now;
                _context.UpazilaCityCorporationThanaMaps.Add(saveUpazilaCityCorporationThanaMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updateThana.ThanaID);
        }
        public async Task<bool> Delete(int ThanaID)
        {
            var data = _context.Thanas.SingleOrDefault(x => x.ThanaID == ThanaID);
            if (data != null)
            {
                var map = _context.UpazilaCityCorporationThanaMaps.SingleOrDefault(map => map.ThanaID == ThanaID);
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
        public async Task<IEnumerable<VMThana>> UpdateOrder(List<VMThana> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMThana Thana in oList)
            {
                var map = _context.UpazilaCityCorporationThanaMaps.SingleOrDefault(x => x.UpazilaCityCorporationThanaMapID == Thana.UpazilaCityCorporationThanaMap.UpazilaCityCorporationThanaMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.UpazilaCityCorporationThanaMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMThana>> Search(QueryObject queryObject)
        {
            var mapList = _context.UpazilaCityCorporationThanaMaps.ToList();
            if (queryObject.UpazilaCityCorporationID > 0)
            {
                mapList = mapList.Where(x => x.UpazilaCityCorporationID == queryObject.UpazilaCityCorporationID).ToList();
            }

            var query =
                from map in mapList
                join Thana in _context.Thanas on map.ThanaID equals Thana.ThanaID
                join UpazilaCityCorporation in _context.UpazilaCityCorporations on map.UpazilaCityCorporationID equals UpazilaCityCorporation.UpazilaCityCorporationID

                select new VMThana
                {
                    ThanaID = Thana.ThanaID,
                    ThanaCode = Thana.ThanaCode,
                    ThanaName = Thana.ThanaName,
                    ThanaNameBangla = Thana.ThanaNameBangla,
                    GeoFenceID = 0,
                    UpazilaCityCorporationThanaMap = map,
                    UpazilaCityCorporationName = UpazilaCityCorporation.UpazilaCityCorporationName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMThana>> GetListByUpazilaCityCorporation(List<VMUpazilaCityCorporation> objList)
        {
            return new List<VMThana>();
            //int selectedOrganizationID = objList[0].OrganizationUpazilaCityCorporationMap.OrganizationID;
            //IEnumerable<string> sUpazilaCityCorporationIDs = objList.Select(x => x.UpazilaCityCorporationID.ToString()).ToList();
            //var lstMap = _context.UpazilaCityCorporationThanaMaps.Where(x => sUpazilaCityCorporationIDs.Contains(x.UpazilaCityCorporationID.ToString())).GroupBy(x => x.ThanaID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.UpazilaCityCorporationThanaMaps.Where(x => sUpazilaCityCorporationIDs.Contains(x.UpazilaCityCorporationID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].UpazilaCityCorporationID == lstMapOrganization[j].UpazilaCityCorporationID && lstMap[i].ThanaID == lstMapOrganization[j].ThanaID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from Thana in _context.Thanas.ToList()
            //join map in lstMap on Thana.ThanaID equals map.ThanaID
            //join UpazilaCityCorporation in objList on map.UpazilaCityCorporationID equals UpazilaCityCorporation.UpazilaCityCorporationID

            //select new VMThana
            //{
            //    ThanaID = Thana.ThanaID,
            //    ThanaCode = Thana.ThanaCode,
            //    ThanaName = Thana.ThanaName,
            //    ThanaNameBangla = Thana.ThanaNameBangla,
            //    GeoFenceID = 0,
            //    UpazilaCityCorporationThanaMap = map,
            //    UpazilaCityCorporationName = UpazilaCityCorporation.UpazilaCityCorporationName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMThana>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sUpazilaIDs = _context.OrganizationUpazilaCityCorporationMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.UpazilaCityCorporationID.ToString()).ToList();
            var lstMap = _context.UpazilaCityCorporationThanaMaps.Where(x => sUpazilaIDs.Contains(x.UpazilaCityCorporationID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationThanaMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join thana in _context.Thanas on map.ThanaID equals thana.ThanaID
                join upazila in _context.UpazilaCityCorporations on map.UpazilaCityCorporationID equals upazila.UpazilaCityCorporationID

                join mapOrganization in lstMapOrganization on map.ThanaID equals mapOrganization.ThanaID into table
                from temp in table.DefaultIfEmpty()
                select new VMThana
                {
                    ThanaID = thana.ThanaID,
                    ThanaCode = thana.ThanaCode,
                    ThanaName = thana.ThanaName,
                    ThanaNameBangla = thana.ThanaNameBangla,
                    GeoFenceID = thana.GeoFenceID,
                    UpazilaCityCorporationName = upazila.UpazilaCityCorporationName,
                    UpazilaCityCorporationThanaMap = map,
                    OrganizationThanaMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x => x.ThanaName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<bool> SaveOrganizationThanaMap(List<OrganizationThanaMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            IEnumerable<string> sOrganizationIDs = oList.Select(x => x.OrganizationID.ToString()).ToList();
            IEnumerable<string> sThanaIDs = oList.Select(x => x.ThanaID.ToString()).ToList();
            var dbMapList = _context.OrganizationThanaMaps.Where(x => sOrganizationIDs.Contains(x.OrganizationID.ToString()) && sThanaIDs.Contains(x.ThanaID.ToString())).ToList();

            for (int i = 0; i < oList.Count; i++)
            {
                var dbData = dbMapList.SingleOrDefault(x => x.OrganizationID == oList[i].OrganizationID && x.ThanaID == oList[i].ThanaID);
                if (dbData == null)
                {
                    if (oList[i].IsActive == true)
                    {
                        OrganizationThanaMap obj = new OrganizationThanaMap();
                        obj.OrganizationID = oList[i].OrganizationID;
                        obj.ThanaID = oList[i].ThanaID;
                        obj.IsActive = true;
                        obj.OrderNo = 0;
                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;
                        _context.OrganizationThanaMaps.Add(obj);
                    }
                }
                else
                {
                    dbData.IsActive = oList[i].IsActive;
                    dbData.OrderNo = oList[i].OrderNo;
                    dbData.UpdatedBy = userId;
                    dbData.UpdatedDate = DateTime.Now;
                    _context.OrganizationThanaMaps.Attach(dbData);
                    _context.Entry(dbData).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return true;
        }
    }
}

