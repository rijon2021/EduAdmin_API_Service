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
using DotNet.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Security.Policy;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;

namespace DotNet.Services.Repositories.Common.AdministrativeUnit
{
    //IGenericRepository<Country>,
    public class CountryRepository : ICountryRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CountryRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        private VMCountry MapObject(Country obj)
        {
            VMCountry country = new VMCountry();
            country.CountryID = obj.CountryID;
            country.CountryCode = obj.CountryCode;
            country.CountryName = obj.CountryName;
            country.CountryNameBangla = obj.CountryNameBangla;
            country.GeoFenceID = obj.GeoFenceID;
            return country;
        }
        public async Task<IEnumerable<VMCountry>> GetAll()
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var dbList = _context.Countrys.ToList();
            List<VMCountry> countryList = new List<VMCountry>();
            if (dbList.Count > 0)
            {
                foreach (Country oItem in dbList)
                {
                    VMCountry country = MapObject(oItem);
                    countryList.Add(country);
                }
            }
            return await Task.FromResult(countryList);
        }
        public async Task<VMCountry> GetByID(int id)
        {
            var result = _context.Countrys.SingleOrDefault(x => x.CountryID == id);
            return await Task.FromResult(MapObject(result));

        }
        public async Task<VMCountry> Add(VMCountry country)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            Country saveCountry = new Country();
            saveCountry.CountryName = country.CountryName;
            saveCountry.CountryCode = country.CountryCode;
            saveCountry.CountryNameBangla = country.CountryNameBangla;
            saveCountry.GeoFenceID = country.GeoFenceID;
            saveCountry.CreatedBy = Convert.ToInt32(userId);
            saveCountry.CreatedDate = DateTime.Now;
            saveCountry.UpdatedBy = Convert.ToInt32(userId);
            saveCountry.UpdatedDate = DateTime.Now;
            _context.Countrys.Add(saveCountry);
            _context.SaveChanges();

            return await GetByID(saveCountry.CountryID);
        }
        public async Task<VMCountry> Update(VMCountry country)
        {
            var data = _context.Countrys.SingleOrDefault(x => x.CountryID == country.CountryID);
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            if (data == null)
            {
                throw new Exception();
            }
            data.CountryName = country.CountryName;
            data.CountryCode = country.CountryCode;
            data.CountryNameBangla = country.CountryNameBangla;
            data.GeoFenceID = country.GeoFenceID;

            _context.Countrys.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();

            return await GetByID(country.CountryID);
        }
        public async Task<bool> Delete(int countryID)
        {
            var data = await GetByID(countryID);
            if (data != null)
            {
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<VMCountry>> Search(QueryObject queryObject)
        {
            var retDataList = await GetAll();
            return await Task.FromResult(retDataList);
        }

        public async Task<IEnumerable<VMCountry>> GetListByOrganization(List<Organization> objList)
        {
            var query =
                from map in _context.OrganizationCountryMaps.ToList()
                join organization in objList on map.OrganizationID equals organization.OrganizationID
                join country in _context.Countrys.ToList() on map.CountryID equals country.CountryID
                select new VMCountry
                {
                    CountryID = country.CountryID,
                    CountryCode = country.CountryCode,
                    CountryName = country.CountryName,
                    CountryNameBangla = country.CountryNameBangla,
                    GeoFenceID = 0,
                    OrganizationCountryMap = map,
                    OrganizationName = organization.OrganizationName,
                };

            var retDataList = query.OrderBy(x => x.CountryName).ToList();
            return await Task.FromResult(retDataList);
        }
        public async Task<bool> SaveOrganizationCountryMap(List<OrganizationCountryMap> oList)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            IEnumerable<string> sOrganizationIDs = oList.Select(x => x.OrganizationID.ToString()).ToList();
            IEnumerable<string> sCountryIDs = oList.Select(x => x.CountryID.ToString()).ToList();
            var dbMapList = _context.OrganizationCountryMaps.Where(x => sOrganizationIDs.Contains(x.OrganizationID.ToString()) && sCountryIDs.Contains(x.CountryID.ToString())).ToList();

            for (int i = 0; i < oList.Count; i++)
            {
                var dbData = dbMapList.SingleOrDefault(x => x.CountryID == oList[i].CountryID && x.OrganizationID == oList[i].OrganizationID);
                if (dbData == null)
                {
                    if (oList[i].IsActive == true)
                    {
                        OrganizationCountryMap obj = new OrganizationCountryMap();
                        obj.OrganizationID = oList[i].OrganizationID;
                        obj.CountryID = oList[i].CountryID;
                        obj.IsActive = true;
                        obj.CreatedBy = userId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedDate = DateTime.Now;
                        _context.OrganizationCountryMaps.Add(obj);
                    }
                }
                else
                {
                    dbData.IsActive = oList[i].IsActive;
                    dbData.UpdatedBy = userId;
                    dbData.UpdatedDate = DateTime.Now;
                    _context.OrganizationCountryMaps.Attach(dbData);
                    _context.Entry(dbData).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return true;
        }
    }
}

