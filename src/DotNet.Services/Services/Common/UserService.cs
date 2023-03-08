using DotNet.ApplicationCore.DTOs;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;
using DotNet.Services.Repositories.Interfaces;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Services.Interfaces.Common;
using DotNet.ApplicationCore.Entities;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using DotNet.Services.Repositories.Common;

namespace DotNet.Services.Services.Common
{
    //GenericRepository<Users>,
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly INotificationAreaRepository _notificationAreaRepository;
        private readonly IOrganizationRepository _organizaionRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        ResponseMessage rm = new ResponseMessage();
        public UserService(
            IUserRepository userRepository,
            ICommonRepository commonRepository,
            INotificationAreaRepository notificationAreaRepository,
            IOrganizationRepository organizaionRepository,
            IUserRoleRepository userRoleRepository
            )// : base(dotnetContext)
        {
            _userRepository = userRepository;
            _commonRepository = commonRepository;
            _notificationAreaRepository = notificationAreaRepository;
            _organizaionRepository = organizaionRepository;
            _userRoleRepository = userRoleRepository;
        }

        public ResponseMessage UserAuthentication(AuthUser user)
        {
            try
            {
                AuthUser authUser = _userRepository.UserAuthentication(user);
                if (authUser.UserAutoID > 0)
                {
                    rm.StatusCode = ReturnStatus.Success;
                    rm.ResponseObj = authUser;
                }
                else
                {
                    rm.Message = "Invalid UserID or Password";
                    rm.StatusCode = ReturnStatus.Failed;
                }
            }
            catch (Exception ex)
            {
                rm.Message = ex.Message;
                rm.StatusCode = ReturnStatus.Failed;
            }
            return rm;
        }
        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _userRepository.GetAll();
        }
        public async Task<Users> GetByID(int id)
        {
            var response = await _userRepository.GetByID(id);
            return response;
        }
        public async Task<Users> Add(Users user)
        {
            var data = await _userRepository.Add(user);
            return data;
        }
        public async Task<Users> Update(Users user)
        {
            var result = await _userRepository.Update(user);
            var notificationArea = await _notificationAreaRepository.GetByID((int)NotificationAreaEnum.UserLogin);
            await _commonRepository.SendSMS(user.UserAutoID, user.MobileNo, "your number changed", notificationArea, "");
            return result;
        }
        public async Task<bool> Delete(int id)
        {
            var response = await _userRepository.Delete(id);
            return response;
        }
        public async Task<IEnumerable<Users>> GetAllByOrganizationID()
        {
            return await _userRepository.GetAllByOrganizationID();
        }
        public async Task<ResponseMessage> GetInitialData()
        {
            try
            {
                var lstOrganization = await _organizaionRepository.GetAll();
                var lstUserRole = await _userRoleRepository.GetAll();

                rm.StatusCode = ReturnStatus.Success;
                rm.ResponseObj = new
                {
                    lstOrganization = lstOrganization,
                    lstUserRole = lstUserRole
                };
            }
            catch (Exception ex)
            {
                rm.Message = ex.Message;
                rm.StatusCode = ReturnStatus.Failed;
            }
            return rm;
        }
    }
}
