using DotNet.ApplicationCore.DTOs;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.Services.Repositories.Interfaces;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Services.Interfaces.Common;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Common;
using DotNet.Infrastructure.Persistence.Contexts;
using DotNet.Services.Repositories.Infrastructure;

namespace DotNet.Services.Services.Common
{
    //GenericRepository<Users>,
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userLevelRepository;

        ResponseMessage rm = new ResponseMessage();
        public UserRoleService(
            IUserRoleRepository UserLevelRepository
            )// : base(dotnetContext)
        {
            _userLevelRepository = UserLevelRepository;
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            return await _userLevelRepository.GetAll();
        }
        public async Task<UserRole> GetByID(int id)
        {
            var response = await _userLevelRepository.GetByID(id);
            return response;
        }
        public async Task<UserRole> Add(UserRole userLevel)
        {
            var data = await _userLevelRepository.Add(userLevel);
            return data;
        }
        public async Task<UserRole> Update(UserRole userLevel)
        {
            return await _userLevelRepository.Update(userLevel);
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _userLevelRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<UserRole>> UpdateOrder(List<UserRole> oList)
        {
            return await _userLevelRepository.UpdateOrder(oList);
        }
    }
}
