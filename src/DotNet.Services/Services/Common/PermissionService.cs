using DotNet.ApplicationCore.DTOs;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.Services.Repositories.Interfaces;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Services.Interfaces.Common;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Common;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.ApplicationCore.DTOs.Common;
using Newtonsoft.Json;

namespace DotNet.Services.Services.Common
{
    //GenericRepository<Users>,
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        ResponseMessage rm = new ResponseMessage();
        public PermissionService(
            IPermissionRepository permissionRepository
            )// : base(dotnetContext)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<Permission>> GetAll()
        {
            return await _permissionRepository.GetAll();
        }
        public async Task<Permission> GetByID(int id)
        {
            var response = await _permissionRepository.GetByID(id);
            return response;
        }
        public async Task<Permission> Add(Permission permission)
        {
            var data = await _permissionRepository.Add(permission);
            return data;
        }
        public async Task<Permission> Update(Permission permission)
        {
            return await _permissionRepository.Update(permission);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _permissionRepository.Delete(id);
            return response;
        }
        public List<PermissionDTO> MakeListWithChild(List<Permission> oList)
        {
            List<PermissionDTO> lstPermissionDTO = new List<PermissionDTO>();
            PermissionDTO permissionDTO = new PermissionDTO();

            if (oList != null && oList.Count > 0)
            {
                foreach(Permission permission in oList)
                {
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(permission);
                    permissionDTO = JsonConvert.DeserializeObject<PermissionDTO>(data);
                    bool hasChild = oList.Any(x => x.ParentPermissionID == permission.PermissionID && x.PermissionType == PermissionType.Menu);
                    if (hasChild)
                    {
                        permissionDTO.HasChild= true;
                    }
                    else
                    {
                        permissionDTO.HasChild = false;
                    }
                    lstPermissionDTO.Add(permissionDTO);
                }
            }
            return lstPermissionDTO;
        }
        public async Task<IEnumerable<Permission>> UpdateOrder(List<Permission> oList)
        {
            return await _permissionRepository.UpdateOrder(oList);
        }
    }
}
