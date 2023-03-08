using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.DTOs.Common;
using DotNet.ApplicationCore.DTOs.VM.AdministrativeUnit;
using DotNet.ApplicationCore.Entities;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Interfaces.Common.AdministrativeUnit
{
    public interface IDivisionService //: ICommonInterface<Division>
    {
        Task<IEnumerable<VMDivision>> GetAll();
        Task<VMDivision> GetByID(int id);
        Task<VMDivision> Add(VMDivision entity);
        Task<VMDivision> Update(VMDivision entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<VMDivision>> UpdateOrder(List<VMDivision> oList);
        Task<IEnumerable<VMDivision>> Search(QueryObject queryObject);
        //Task<IEnumerable<VMDivision>> GetListByOrganization(int id);
        Task<IEnumerable<VMDivision>> GetListByCountry(List<VMCountry> objList);
        Task<IEnumerable<VMDivision>> GetListByOrganizationID(int id);


    }
}
