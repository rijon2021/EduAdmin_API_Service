using DotNet.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Services.Infrastructure
{
    public interface ICommonInterface<T> where T : class
    {
        //Task<IEnumerable<T>> GetAll();
        //Task<T> GetByID(int id);
        //Task<T> Add(T entity);
        //Task<T> Update(T entity);
        //Task<bool> SendSMS(int UserID, string recipientMobileNo, string messageBody, NotificationArea notificationArea, string otp);
        //Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
    }
}
