using DotNet.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;

namespace DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit
{

    public interface IDivisionRepository //: ICommonRepository<Division>
    {
        Task<IEnumerable<VMDivision>> GetAll();
        Task<VMDivision> GetByID(int id);
        Task<VMDivision> Add(VMDivision entity);
        Task<VMDivision> Update(VMDivision entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMDivision>> UpdateOrder(List<VMDivision> oList);
        Task<IEnumerable<VMDivision>> Search(QueryObject queryObject);
        Task<IEnumerable<VMDivision>> GetListByCountry(List<VMCountry> objList);
        Task<IEnumerable<VMDivision>> GetListByOrganizationID(int id);
        Task<bool> SaveOrganizationDivisionMap(List<OrganizationDivisionMap> oList);


        


    }
}
