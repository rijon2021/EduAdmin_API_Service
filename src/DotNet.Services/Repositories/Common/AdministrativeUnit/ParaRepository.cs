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
    public class ParaRepository : IParaRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ParaRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VMPara>> GetAll()
        {
            var query =
                from para in _context.Paras.ToList()
                join map in _context.VillageAreaParaMaps.ToList() on para.ParaID equals map.ParaID
                join villageArea in _context.VillageAreas.ToList() on map.VillageAreaID equals villageArea.VillageAreaID

                select new VMPara
                {
                    ParaID = para.ParaID,
                    ParaCode = para.ParaCode,
                    ParaName = para.ParaName,
                    ParaNameBangla = para.ParaNameBangla,
                    GeoFenceID = para.GeoFenceID == null? 0 : para.GeoFenceID ,
                    VillageAreaParaMap = map,
                    VillageAreaName = villageArea.VillageAreaName,
                };
            var retDataList = query.OrderBy(x => x.VillageAreaName).ThenBy(x => x.ParaName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<VMPara> GetByID(int id)
        {
            var result = _context.Paras.SingleOrDefault(x => x.ParaID == id);
            var resultMap = _context.VillageAreaParaMaps.SingleOrDefault(x => x.ParaID == id);

            VMPara retObj = new VMPara();
            retObj.ParaID = result.ParaID;
            retObj.ParaName = result.ParaName;
            retObj.ParaCode = result.ParaCode;
            retObj.ParaNameBangla = result.ParaNameBangla;
            retObj.GeoFenceID = result.GeoFenceID;
            retObj.VillageAreaParaMap = resultMap;

            return await Task.FromResult(retObj);
        }
        public async Task<VMPara> Add(VMPara para)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var existsData = _context.Paras.SingleOrDefault(x => x.ParaCode == para.ParaCode);
            if (existsData != null)
            {
                throw new Exception("Data Exists with this code !");
            }

            _context.Database.BeginTransaction();
            Para savePara = new Para();
            savePara.ParaName = para.ParaName;
            savePara.ParaCode = para.ParaCode;
            savePara.ParaNameBangla = para.ParaNameBangla;
            savePara.GeoFenceID = para.GeoFenceID;
            savePara.CreatedBy = Convert.ToInt32(userID);
            savePara.CreatedDate = DateTime.Now;
            savePara.UpdatedBy = Convert.ToInt32(userID);
            savePara.UpdatedDate = DateTime.Now;
            _context.Paras.Add(savePara);
            _context.SaveChanges();

            VillageAreaParaMap saveVillageAreaParaMap = new VillageAreaParaMap();
            saveVillageAreaParaMap.VillageAreaID = para.VillageAreaParaMap.VillageAreaID;
            saveVillageAreaParaMap.ParaID = savePara.ParaID;
            saveVillageAreaParaMap.ValidityDate = DateTime.Now.AddYears(1);
            saveVillageAreaParaMap.IsActive = para.VillageAreaParaMap.IsActive;
            saveVillageAreaParaMap.CreatedBy = Convert.ToInt32(userID);
            saveVillageAreaParaMap.CreatedDate = DateTime.Now;
            saveVillageAreaParaMap.UpdatedBy = Convert.ToInt32(userID);
            saveVillageAreaParaMap.UpdatedDate = DateTime.Now;
            _context.VillageAreaParaMaps.Add(saveVillageAreaParaMap);
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(savePara.ParaID);
        }
        public async Task<VMPara> Update(VMPara para)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();

            var updatePara = _context.Paras.SingleOrDefault(x => x.ParaID == para.ParaID);
            if (updatePara == null)
            {
                throw new Exception();
            }
            var duplicateData = _context.Paras.SingleOrDefault(x => x.ParaCode == para.ParaCode && x.ParaID != para.ParaID);
            if (duplicateData != null)
            {
                throw new Exception("Data Exists with this code !");
            }
            _context.Database.BeginTransaction();
            updatePara.ParaName = para.ParaName;
            updatePara.ParaCode = para.ParaCode;
            updatePara.ParaNameBangla = para.ParaNameBangla;
            updatePara.GeoFenceID = para.GeoFenceID;
            updatePara.UpdatedBy = userID;
            updatePara.UpdatedDate = DateTime.Now;
            _context.Paras.Attach(updatePara);
            _context.Entry(updatePara).State = EntityState.Modified;

            var updateVillageAreaParaMap = _context.VillageAreaParaMaps.SingleOrDefault(x => x.ParaID == para.ParaID);
            if (updateVillageAreaParaMap != null)
            {
                updateVillageAreaParaMap.VillageAreaID = para.VillageAreaParaMap.VillageAreaID;
                updateVillageAreaParaMap.ParaID = updatePara.ParaID;
                updateVillageAreaParaMap.ValidityDate = para.VillageAreaParaMap.ValidityDate;
                updateVillageAreaParaMap.IsActive = para.VillageAreaParaMap.IsActive;
                updateVillageAreaParaMap.UpdatedBy = Convert.ToInt32(userID);
                updateVillageAreaParaMap.UpdatedDate = DateTime.Now;
                _context.Attach(updateVillageAreaParaMap);
                _context.Entry(updateVillageAreaParaMap).State = EntityState.Modified;
            }
            else
            {
                VillageAreaParaMap saveVillageAreaParaMap = new VillageAreaParaMap();
                saveVillageAreaParaMap.VillageAreaID = para.VillageAreaParaMap.VillageAreaID;
                saveVillageAreaParaMap.ParaID = updatePara.ParaID;
                saveVillageAreaParaMap.ValidityDate = DateTime.Now.AddYears(1);
                saveVillageAreaParaMap.IsActive = para.VillageAreaParaMap.IsActive;
                saveVillageAreaParaMap.CreatedBy = Convert.ToInt32(userID);
                saveVillageAreaParaMap.CreatedDate = DateTime.Now;
                saveVillageAreaParaMap.UpdatedBy = Convert.ToInt32(userID);
                saveVillageAreaParaMap.UpdatedDate = DateTime.Now;
                _context.VillageAreaParaMaps.Add(saveVillageAreaParaMap);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();

            return await GetByID(updatePara.ParaID);
        }
        public async Task<bool> Delete(int ParaID)
        {
            var data = _context.Paras.SingleOrDefault(x => x.ParaID == ParaID);
            if (data != null)
            {
                var map = _context.VillageAreaParaMaps.SingleOrDefault(map => map.ParaID == ParaID);
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
        public async Task<IEnumerable<VMPara>> UpdateOrder(List<VMPara> oList)
        {
            var userID = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            foreach (VMPara Para in oList)
            {
                var map = _context.VillageAreaParaMaps.SingleOrDefault(x => x.VillageAreaParaMapID == Para.VillageAreaParaMap.VillageAreaParaMapID);
                if (map != null)
                {
                    map.UpdatedBy = userID;
                    map.UpdatedDate = DateTime.Now;
                    _context.VillageAreaParaMaps.Attach(map);
                    _context.Entry(map).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return await GetAll();
        }
        public async Task<IEnumerable<VMPara>> Search(QueryObject queryObject)
        {
            var mapList = _context.VillageAreaParaMaps.ToList();
            if (queryObject.VillageAreaID > 0)
            {
                mapList = mapList.Where(x => x.VillageAreaID == queryObject.VillageAreaID).ToList();
            }

            var query =
                from map in mapList
                join Para in _context.Paras on map.ParaID equals Para.ParaID
                join VillageArea in _context.VillageAreas on map.VillageAreaID equals VillageArea.VillageAreaID

                select new VMPara
                {
                    ParaID = Para.ParaID,
                    ParaCode = Para.ParaCode,
                    ParaName = Para.ParaName,
                    ParaNameBangla = Para.ParaNameBangla,
                    GeoFenceID = 0,
                    VillageAreaParaMap = map,
                    VillageAreaName = VillageArea.VillageAreaName,
                };

            var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<IEnumerable<VMPara>> GetListByVillageArea(List<VMVillageArea> objList)
        {
            return new List<VMPara>();
            //int selectedOrganizationID = objList[0].OrganizationVillageAreaMap.OrganizationID;
            //IEnumerable<string> sVillageAreaIDs = objList.Select(x => x.VillageAreaID.ToString()).ToList();
            //var lstMap = _context.VillageAreaParaMaps.Where(x => sVillageAreaIDs.Contains(x.VillageAreaID.ToString())).GroupBy(x => x.ParaID, (key, group) => group.First()).ToList();
            //var lstMapOrganization = _context.VillageAreaParaMaps.Where(x => sVillageAreaIDs.Contains(x.VillageAreaID.ToString()) && x.OrganizationID == selectedOrganizationID).ToList();

            //for (int i = 0; i < lstMap.Count; i++)
            //{
            //    lstMap[i].IsActive = false;
            //    for (int j = 0; j < lstMapOrganization.Count; j++)
            //    {
            //        if (lstMap[i].VillageAreaID == lstMapOrganization[j].VillageAreaID && lstMap[i].ParaID == lstMapOrganization[j].ParaID)
            //        {
            //            lstMap[i].OrganizationID = lstMapOrganization[j].OrganizationID;
            //            lstMap[i].IsActive = lstMapOrganization[j].IsActive;
            //            lstMap[i].OrderNo = lstMapOrganization[j].OrderNo;
            //            lstMap[i].ValidityDate = lstMapOrganization[j].ValidityDate;
            //        }
            //    }
            //}


            //var query =
            //from Para in _context.Paras.ToList()
            //join map in lstMap on Para.ParaID equals map.ParaID
            //join VillageArea in objList on map.VillageAreaID equals VillageArea.VillageAreaID

            //select new VMPara
            //{
            //    ParaID = Para.ParaID,
            //    ParaCode = Para.ParaCode,
            //    ParaName = Para.ParaName,
            //    ParaNameBangla = Para.ParaNameBangla,
            //    GeoFenceID = 0,
            //    VillageAreaParaMap = map,
            //    VillageAreaName = VillageArea.VillageAreaName,
            //    OrderNo = map.OrderNo,
            //    IsChecked = map.IsActive
            //};
            //var retDataList = query.OrderBy(x => x.OrderNo).ToList();
            //return await Task.FromResult(retDataList);


        }
        public async Task<IEnumerable<VMPara>> GetListByOrganizationID(int organizationID)
        {
            IEnumerable<string> sVillageIDs = _context.OrganizationVillageAreaMaps.Where(x => x.OrganizationID == organizationID && x.IsActive == true).Select(x => x.VillageAreaID.ToString()).ToList();
            var lstMap = _context.VillageAreaParaMaps.Where(x => sVillageIDs.Contains(x.VillageAreaID.ToString())).ToList();
            var lstMapOrganization = _context.OrganizationParaMaps.Where(x => x.OrganizationID == organizationID).ToList();

            var query =
                from map in lstMap
                join para in _context.Paras on map.ParaID equals para.ParaID
                join village in _context.VillageAreas on map.VillageAreaID equals village.VillageAreaID

                join mapOrganization in lstMapOrganization on map.ParaID equals mapOrganization.ParaID into table
                from temp in table.DefaultIfEmpty()
                select new VMPara
                {
                    ParaID = para.ParaID,
                    ParaCode = para.ParaCode,
                    ParaName = para.ParaName,
                    ParaNameBangla = para.ParaNameBangla,
                    GeoFenceID = para.GeoFenceID,
                    VillageAreaName = village.VillageAreaName,
                    VillageAreaParaMap = map,
                    OrganizationParaMap = (temp == null) ? null : temp,
                    OrderNo = (temp == null) ? 0 : temp.OrderNo,
                    IsChecked = (temp == null) ? false : temp.IsActive,
                };

            var retDataList = query.OrderByDescending(x => x.IsChecked).ThenBy(x => x.ParaName).ToList();
            return await Task.FromResult(retDataList);

        }
        public async Task<bool> SaveOrganizationParaMap(List<OrganizationParaMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            IEnumerable<string> sOrganizationIDs = oList.Select(x => x.OrganizationID.ToString()).ToList();
            IEnumerable<string> sParaIDs = oList.Select(x => x.ParaID.ToString()).ToList();
            var dbMapList = _context.OrganizationParaMaps.Where(x => sOrganizationIDs.Contains(x.OrganizationID.ToString()) && sParaIDs.Contains(x.ParaID.ToString())).ToList();

            for (int i = 0; i < oList.Count; i++)
            {
                var dbData = dbMapList.SingleOrDefault(x => x.OrganizationID == oList[i].OrganizationID && x.ParaID == oList[i].ParaID);
                if (dbData == null)
                {
                    if (oList[i].IsActive == true)
                    {
                        OrganizationParaMap obj = new OrganizationParaMap();
                        obj.OrganizationID = oList[i].OrganizationID;
                        obj.ParaID = oList[i].ParaID;
                        obj.OrderNo = oList[i].OrderNo;
                        obj.IsActive = true;
                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;
                        _context.OrganizationParaMaps.Add(obj);
                    }
                }
                else
                {
                    dbData.IsActive = oList[i].IsActive;
                    dbData.OrderNo = oList[i].OrderNo;
                    dbData.UpdatedBy = userId;
                    dbData.UpdatedDate = DateTime.Now;
                    _context.OrganizationParaMaps.Attach(dbData);
                    _context.Entry(dbData).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return true;

        }
    }
}

