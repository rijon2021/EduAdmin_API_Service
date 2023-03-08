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

namespace DotNet.Services.Repositories.Common
{
    public class UserRepository : IUserRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }
        public AuthUser UserAuthentication(AuthUser user)
        {     
            string password = EncryptionHelper.Encrypt(user.Password);
            Users dbUser = _context.Users.SingleOrDefault(x => string.Equals(x.UserID, user.UserID) && string.Equals(x.Password, password));
            AuthUser authUser = new AuthUser();
            if (dbUser != null)
            {
                authUser.UserAutoID = dbUser.UserAutoID;
                authUser.UserID = dbUser.UserID;
                authUser.UserTypeID = dbUser.UserTypeID;
                authUser.OrganizationID = dbUser.OrganizationID;
                authUser.UserFullName = dbUser.UserFullName;
                authUser.UserRoleID = dbUser.UserRoleID;
            }
            return authUser;
        }
        public async Task<IEnumerable<Users>> GetAll()
        {
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            int userAutoId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var users = _context.Users.Where(x => x.OrganizationID == organizationID).ToList();
            return await Task.FromResult(users);
        }
        public async Task<Users> GetByID(int id)
        {
            var result = _context.Users.SingleOrDefault(x => x.UserAutoID == id);
            return await Task.FromResult(result);
        }
        public async Task<Users> Add(Users user)
        {
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            user.Password = EncryptionHelper.Encrypt(user.Password);
            user.PasswordExpiryDate = DateTime.Now;
            user.CreatedBy = Convert.ToInt32(userId);
            user.CreatedDate = DateTime.Now;
            user.UpdatedBy = Convert.ToInt32(userId);
            user.UpdatedDate = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();

            return await GetByID(user.UserAutoID);
        }
        public async Task<Users> Update(Users user)
        {
            var data = await GetByID(user.UserAutoID);
            var userId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();

            if (data == null)
            {
                throw new Exception();
            }
            data.UserID = user.UserID;
            data.UserTypeID = user.UserTypeID;
            data.OrganizationID = user.OrganizationID;
            data.UserFullName = user.UserFullName;
            data.MobileNo = user.MobileNo;
            data.Address = user.Address;
            data.PasswordExpiryDate = user.PasswordExpiryDate;
            data.Status = user.Status;
            data.Email = user.Email;
            data.UserRoleID= user.UserRoleID;
            data.UserImage = user.UserImage;
            data.LastLatitude = user.LastLatitude;
            data.LastLongitude = user.LastLongitude;
            data.Is2FAauthenticationEnabled = user.Is2FAauthenticationEnabled;
            data.NID = user.NID;
            data.CanChangeOwnPassword = user.CanChangeOwnPassword;
            data.MobileVerification = user.MobileVerification;
            data.UpdatedBy = Convert.ToInt32(userId);
            data.UpdatedDate = DateTime.Now;

            _context.Users.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
            _context.SaveChanges();

            return data;
        }
        public async Task<bool> Delete(int userAutoID)
        {
            var data = await GetByID(userAutoID);
            if (data != null)
            {
                _context.Entry(data).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<Users>> GetAllByOrganizationID()
        {
            int organizationID = await _httpContextAccessor.HttpContext.User.GetOrginzationIdFromClaimIdentity();
            int userAutoId = await _httpContextAccessor.HttpContext.User.GetUserAutoIdFromClaimIdentity();
            var users = _context.Users.Where(x => x.OrganizationID == organizationID).ToList();
            return await Task.FromResult(users);
        }
    }
}

