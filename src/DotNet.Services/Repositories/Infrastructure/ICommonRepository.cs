
using DotNet.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Repositories.Infrastructure
{
    public interface ICommonRepository
    {
        Task<bool> SendSMS(int UserID, string recipientMobileNo, string messageBody, NotificationArea notificationArea, string otp);
           
        //Task<IEnumerable<T>> GetAll();
        //Task<T> GetByID(int id);
        //Task<T> Add(T entity);
        //Task<T> Update(T entity);
        //Task<bool> Delete(int id);
        //Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
    }
}